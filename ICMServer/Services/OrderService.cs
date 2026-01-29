using ICMServer.DBContext;
using ICMServer.Interfaces;
using ICMServer.Models;
using Microsoft.EntityFrameworkCore;

namespace ICMServer.Services
{
    public class OrderService : IOrderService
    {
        private readonly ICMDBContext _context;
        private readonly ILogger<OrderService> _logger;

        public OrderService(ICMDBContext context, ILogger<OrderService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<OrderReprocessResult>> ReprocessOrdersAsync(List<string> orderNumbers)
        {
            var results = new List<OrderReprocessResult>();

            // Get system period
            var systemYear = await GetSystemParameter("PERIOD_YEAR");
            var systemMonth = await GetSystemParameter("PERIOD_MONTH");

            if (string.IsNullOrEmpty(systemYear) || string.IsNullOrEmpty(systemMonth))
            {
                throw new InvalidOperationException("System period parameters (PERIOD_YEAR / PERIOD_MONTH) not configured.");
            }

            foreach (var orderNumber in orderNumbers)
            {
                var trimmed = orderNumber.Trim();
                if (!string.IsNullOrEmpty(trimmed))
                {
                    var result = await ReprocessSingleOrderAsync(trimmed, systemYear, systemMonth);
                    results.Add(result);
                }
            }

            return results;
        }

        private async Task<OrderReprocessResult> ReprocessSingleOrderAsync(string orderNumber, string systemYear, string systemMonth)
        {
            try
            {
                // Find order by order number
                var order = await _context.DataOrderHeaders
                    .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);

                if (order == null)
                {
                    var msg = $"Order {orderNumber} doesn't exist in current sp ({systemYear}{systemMonth})";
                    await WriteSystemLog("[usp_REPROCESS_ORDER]", "WARN", "Reprocess order", msg);
                    return new OrderReprocessResult { OrderNumber = orderNumber, Success = false, Message = msg };
                }

                var orderId = order.OrderRowId;
                var orderMonth = order.PeriodMonth;
                var orderYear = order.PeriodYear;

                // Check if order belongs to current sales period
                if (orderMonth != systemMonth || orderYear != systemYear)
                {
                    string msg;
                    if (string.IsNullOrEmpty(orderYear) || string.IsNullOrEmpty(orderMonth))
                        msg = $"Order {orderNumber} doesn't exist to current sp ({systemYear}{systemMonth})";
                    else
                        msg = $"Order {orderNumber}({orderYear}{orderMonth}) doesn't exist to current sp ({systemYear}{systemMonth})";

                    await WriteSystemLog("[usp_REPROCESS_ORDER]", "WARN", "Reprocess order", msg);
                    return new OrderReprocessResult { OrderNumber = orderNumber, Success = false, Message = msg };
                }

                await WriteSystemLog("[usp_REPROCESS_ORDER]", "INFO", $"{orderNumber},{orderId}", "Order id found from order number");

                // Delete credit allocations for this order (non-adjustment)
                var creditAllocations = await _context.DataCreditAllocations
                    .Where(c => c.OrderId == orderId
                        && c.PeriodYear == systemYear
                        && c.PeriodMonth == systemMonth
                        && c.AdjustmentId == null)
                    .ToListAsync();
                if (creditAllocations.Count > 0)
                    _context.DataCreditAllocations.RemoveRange(creditAllocations);

                // Delete commission payments for this order (non-adjustment)
                var commissionPayments = await _context.DataCommissionPayments
                    .Where(c => c.OrderId == orderId
                        && c.PeriodYear == systemYear
                        && c.PeriodMonth == systemMonth
                        && c.AdjustmentId == null)
                    .ToListAsync();
                if (commissionPayments.Count > 0)
                    _context.DataCommissionPayments.RemoveRange(commissionPayments);

                // Reset process history flags
                var processHistories = await _context.DataOrderProcessHistories
                    .Where(h => h.RowId == orderId)
                    .ToListAsync();
                foreach (var ph in processHistories)
                {
                    ph.RevenueProcessed = false;
                    ph.CommissionProcessed = false;
                }

                await _context.SaveChangesAsync();

                var successMsg = $"Order {orderNumber}({orderYear}{orderMonth}) in current sp ({systemYear}{systemMonth}). Reprocessing done. Please run ICM";
                _logger.LogInformation(successMsg);

                return new OrderReprocessResult { OrderNumber = orderNumber, Success = true, Message = successMsg };
            }
            catch (Exception ex)
            {
                var errorMsg = $"Error occurred with reprocessing order {orderNumber}. Reason: {ex.Message}";
                _logger.LogError(ex, errorMsg);
                return new OrderReprocessResult { OrderNumber = orderNumber, Success = false, Message = errorMsg };
            }
        }

        /// <summary>
        /// Get full order summary by order number
        /// </summary>
        public async Task<OrderSummaryDto?> GetOrderSummaryAsync(string orderNumber)
        {
            // 1. Find order
            var order = await _context.DataOrderHeaders
                .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);

            if (order == null) return null;

            var orderId = order.OrderRowId;

            // 2. Order Header with primary position employee name
            var primaryEmployee = await (
                from dop in _context.DataOrderPositions
                join de in _context.DataEmployees on dop.EmployeeRowId equals de.RowId
                where dop.OrderRowId == orderId
                select new { Name = ((de.FstName ?? "") + " " + (de.LastName ?? "")).Trim() }
            ).FirstOrDefaultAsync();

            var header = new OrderHeaderDto
            {
                OrderRowId = order.OrderRowId,
                OrderNumber = order.OrderNumber,
                OrderType = order.OrderType,
                PromotionCode = order.PromotionCode ?? "N/A",
                SapOrderReference = order.SapOrderReference ?? "N/A",
                CustomerAccountNumber = order.CustomerAccountNumber,
                CustomerName = order.CustomerName,
                CustomerType = order.CustomerType,
                PrimaryPosition = primaryEmployee?.Name,
                OrderListVal = order.OrderListVal,
                OrderSaleVal = order.OrderSaleVal,
                OrderDiscountVal = order.OrderDiscountVal,
                OrderDiscountPercent = order.OrderDiscountPercent,
                ServiceListVal = order.ServiceListVal,
                ServiceSaleVal = order.ServiceSaleVal,
                ServiceDiscountVal = order.ServiceDiscountVal,
                ServiceDiscountPercent = order.ServiceDiscountPercent,
                ServiceType = order.ServiceType,
                PeriodMonth = order.PeriodMonth,
                PeriodYear = order.PeriodYear,
                MaintenanceTerm = order.MaintenanceTerm
            };

            // 3. Order Lines
            var lines = await _context.DataOrderItems
                .Where(i => i.OrderRowId == orderId)
                .Select(i => new OrderLineDto
                {
                    OrderItemId = i.OrderItemId,
                    LineNumber = i.LineNumber,
                    ProductCode = i.ProductCode,
                    ProductDesc = i.ProductDesc,
                    ListValue = i.ListValue,
                    SaleValue = i.SaleValue,
                    Quantity = i.Quantity,
                    ProductLevel1 = i.ProductLevel1,
                    ProductLevel2 = i.ProductLevel2,
                    ProductLevel3 = i.ProductLevel3,
                    ProductType = i.ProductType
                })
                .ToListAsync();

            // 4. Allocation Percentages
            var allocations = await (
                from dop in _context.DataOrderPositions
                join de in _context.DataEmployees on dop.EmployeeRowId equals de.RowId
                where dop.OrderRowId == orderId
                select new AllocationPercentDto
                {
                    EmployeeName = ((de.FstName ?? "") + " " + (de.LastName ?? "")).Trim(),
                    EmployeeRowId = dop.EmployeeRowId,
                    PositionName = dop.PositionName,
                    PayplanType = dop.PayplanType,
                    AllocationPercentage = dop.AllocationPercentage
                }
            ).ToListAsync();

            // 5. Commission Payments (non-adjustment: AdjustmentId IS NULL)
            var paymentsRaw = await (
                from cp in _context.DataCommissionPayments
                where cp.OrderId == orderId && cp.AdjustmentId == null
                join de in _context.DataEmployees on cp.EmployeeId equals de.RowId into deGroup
                from de in deGroup.DefaultIfEmpty()
                join enh in _context.ConfigPayplanEnhancements on cp.EnhancementId equals enh.EnhancementId into enhGroup
                from enh in enhGroup.DefaultIfEmpty()
                join rt in _context.ConfigPayplanRateTables on cp.PayplanRateId equals rt.RateTableId into rtGroup
                from rt in rtGroup.DefaultIfEmpty()
                select new { cp, de, enh, rt, cp.PositionId }
            ).ToListAsync();

            // Resolve position names from TmpDataPositions
            var positionIds = paymentsRaw.Select(p => p.PositionId).Where(p => p != null).Distinct().ToList();
            var positionNames = await _context.TmpDataPositions
                .Where(p => positionIds.Contains(p.RowId))
                .ToDictionaryAsync(p => p.RowId!, p => p.PositionName);

            var payments = paymentsRaw.Select(p => new CommissionPaymentDto
            {
                PaymentSource = p.cp.PaymentSource,
                PaymentDescription = p.cp.PaymentDescription,
                PositionId = p.cp.PositionId,
                PositionName = p.cp.PositionId != null && positionNames.ContainsKey(p.cp.PositionId) ? positionNames[p.cp.PositionId] : null,
                EmployeeId = p.cp.EmployeeId,
                EmployeeName = p.de != null ? ((p.de.FstName ?? "") + " " + (p.de.LastName ?? "")).Trim() : null,
                EnhancementDesc = p.enh?.EnhancementDesc,
                RateTableDesc = p.rt?.RateTableDesc,
                PaymentRate = p.cp.PaymentRate,
                PaymentValue = p.cp.PaymentValue,
                PaymentWithheld = p.cp.PaymentWitheld,
                PeriodYear = p.cp.PeriodYear,
                PeriodMonth = p.cp.PeriodMonth
            }).ToList();

            // 6. Adjustments (AdjustmentId IS NOT NULL)
            var adjRaw = await (
                from cp in _context.DataCommissionPayments
                where cp.OrderId == orderId && cp.AdjustmentId != null
                join de in _context.DataEmployees on cp.EmployeeId equals de.RowId into deGroup
                from de in deGroup.DefaultIfEmpty()
                select new { cp, de }
            ).ToListAsync();

            var adjPositionIds = adjRaw.Select(a => a.cp.PositionId).Where(p => p != null).Distinct().ToList();
            var adjPositionNames = await _context.TmpDataPositions
                .Where(p => adjPositionIds.Contains(p.RowId))
                .ToDictionaryAsync(p => p.RowId!, p => p.PositionName);

            var adjustments = adjRaw.Select(a => new AdjustmentDto
            {
                PaymentSource = a.cp.PaymentSource,
                PaymentDescription = a.cp.PaymentDescription,
                PositionId = a.cp.PositionId,
                PositionName = a.cp.PositionId != null && adjPositionNames.ContainsKey(a.cp.PositionId) ? adjPositionNames[a.cp.PositionId] : null,
                EmployeeId = a.cp.EmployeeId,
                EmployeeName = a.de != null ? ((a.de.FstName ?? "") + " " + (a.de.LastName ?? "")).Trim() : null,
                PaymentValue = a.cp.PaymentValue,
                PaymentWithheld = a.cp.PaymentWitheld,
                PeriodMonth = a.cp.PeriodMonth,
                PeriodYear = a.cp.PeriodYear
            }).ToList();

            // 7. Credit Allocations (source = ORDER_ITEM)
            var creditAllocations = await GetCreditAllocationsAsync(orderId, "ORDER_ITEM");

            // 8. Rollups (source = ROLLUP) — join on POSITION_ID instead of EMPLOYEE_ID
            var rollups = await GetRollupsAsync(orderId);

            return new OrderSummaryDto
            {
                Header = header,
                Lines = lines,
                Allocations = allocations,
                Payments = payments,
                Adjustments = adjustments,
                CreditAllocations = creditAllocations,
                Rollups = rollups
            };
        }

        private async Task<List<CreditAllocationDto>> GetCreditAllocationsAsync(string orderId, string source)
        {
            return await (
                from dca in _context.DataCreditAllocations
                where dca.OrderId == orderId && dca.AllocationSource == source
                join de in _context.DataEmployees on dca.EmployeeId equals de.RowId into deGroup
                from de in deGroup.DefaultIfEmpty()
                join doi in _context.DataOrderItems on dca.ItemId equals doi.OrderItemId into doiGroup
                from doi in doiGroup.DefaultIfEmpty()
                join cat in _context.ConfigAllocationTypes on dca.AllocationTypeId equals cat.AllocationTypeId into catGroup
                from cat in catGroup.DefaultIfEmpty()
                select new CreditAllocationDto
                {
                    EmployeeName = de != null ? ((de.FstName ?? "") + " " + (de.LastName ?? "")).Trim() : null,
                    ProductDesc = doi != null ? doi.ProductDesc : null,
                    AllocationDescription = cat != null ? cat.AllocationDescription : null,
                    AllocationList = dca.AllocationList ?? 0,
                    AllocationValue = dca.AllocationValue,
                    ExcludeFromCalcs = dca.ExcludeFromCalcs == "0" || dca.ExcludeFromCalcs == null ? "NO" : "YES",
                    RollupProcessed = dca.RollupProcessed,
                    PeriodMonth = dca.PeriodMonth,
                    PeriodYear = dca.PeriodYear
                }
            ).ToListAsync();
        }

        private async Task<List<CreditAllocationDto>> GetRollupsAsync(string orderId)
        {
            // Rollups join on PositionId = DE.PR_HELD_POSTN_ID (not EmployeeId)
            return await (
                from dca in _context.DataCreditAllocations
                where dca.OrderId == orderId && dca.AllocationSource == "ROLLUP"
                join de in _context.DataEmployees on dca.PositionId equals de.PrHeldPostnId into deGroup
                from de in deGroup.DefaultIfEmpty()
                join doi in _context.DataOrderItems on dca.ItemId equals doi.OrderItemId into doiGroup
                from doi in doiGroup.DefaultIfEmpty()
                join cat in _context.ConfigAllocationTypes on dca.AllocationTypeId equals cat.AllocationTypeId into catGroup
                from cat in catGroup.DefaultIfEmpty()
                select new CreditAllocationDto
                {
                    EmployeeName = de != null ? ((de.FstName ?? "") + " " + (de.LastName ?? "")).Trim() : null,
                    ProductDesc = doi != null ? doi.ProductDesc : null,
                    AllocationDescription = cat != null ? cat.AllocationDescription : null,
                    AllocationList = dca.AllocationList ?? 0,
                    AllocationValue = dca.AllocationValue,
                    ExcludeFromCalcs = dca.ExcludeFromCalcs == "0" || dca.ExcludeFromCalcs == null ? "NO" : "YES",
                    RollupProcessed = dca.RollupProcessed,
                    PeriodMonth = dca.PeriodMonth,
                    PeriodYear = dca.PeriodYear
                }
            ).ToListAsync();
        }

        private async Task<string?> GetSystemParameter(string parameterName)
        {
            return await _context.ConfigSystemParameters
                .Where(p => p.ParameterName == parameterName)
                .Select(p => p.ParameterValue)
                .FirstOrDefaultAsync();
        }

        private async Task WriteSystemLog(string source, string refType, string refValue, string desc)
        {
            _context.SystemLogs.Add(new SystemLog
            {
                LogSource = source,
                ReferenceType = refType,
                ReferenceValue = refValue,
                LogDesc = desc,
                LogDatetime = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
        }
    }
}

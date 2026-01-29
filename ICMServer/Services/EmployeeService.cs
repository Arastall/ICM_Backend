using ICMServer.DBContext;
using ICMServer.Interfaces;
using ICMServer.Models;
using Microsoft.EntityFrameworkCore;

namespace ICMServer.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ICMDBContext _context;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(ICMDBContext context, ILogger<EmployeeService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<EmployeeSummaryDto?> GetEmployeeSummaryAsync(string login, string year, string month)
        {
            // 1. Employee Details
            var empDetail = await (
                from de in _context.DataEmployees
                join tdp in _context.TmpDataPositions on de.PrHeldPostnId equals tdp.RowId into tdpGroup
                from tdp in tdpGroup.DefaultIfEmpty()
                where de.Login != null && de.Login.ToUpper() == login.ToUpper()
                select new EmployeeDetailDto
                {
                    RowId = de.RowId,
                    Login = de.Login,
                    FstName = de.FstName,
                    LastName = de.LastName,
                    JobTitle = de.JobTitle,
                    EmployeeNumber = de.EmployeeNumber,
                    PositionName = tdp != null ? tdp.PositionName : null,
                    XRepType = tdp != null ? tdp.XRepType : null
                }
            ).FirstOrDefaultAsync();

            if (empDetail == null) return null;

            var empid = empDetail.RowId;

            // 2. Get posid
            var posid = await _context.TmpDataPositions
                .Where(p => p.EndDt == null
                    && p.XRepType != null
                    && p.PositionName != null && !p.PositionName.EndsWith("-CT")
                    && p.PrEmpId == empid)
                .Select(p => p.RowId)
                .FirstOrDefaultAsync();

            // 3. Allocated Orders
            var allocatedOrders = await (
                from dca in _context.DataCreditAllocations
                join doh in _context.DataOrderHeaders on dca.OrderId equals doh.OrderRowId
                join doi in _context.DataOrderItems on new { OrderId = dca.OrderId, ItemId = dca.ItemId } equals new { OrderId = doi.OrderRowId, ItemId = doi.OrderItemId }
                join dop in _context.DataOrderPositions on new { OrderId = dca.OrderId, PositionId = dca.PositionId } equals new { OrderId = dop.OrderRowId, PositionId = dop.PositionRowId }
                where dca.AllocationTypeId == 1
                    && dca.AllocationSource != "ROLLUP"
                    && dca.ExcludeFromCalcs == "0"
                    && dca.EmployeeId == empid
                    && dca.PeriodMonth == month
                    && dca.PeriodYear == year
                group new { dca, doh, dop } by new
                {
                    doh.SapOrderReference,
                    doh.CustomerName,
                    doh.OrderNumber,
                    dop.PositionName,
                    dop.PayplanType,
                    doh.OrderDiscountPercent,
                    doh.ServiceType,
                    doh.ServiceDiscountPercent,
                    doh.OrderType,
                    doh.CustomerType
                } into g
                select new AllocatedOrderDto
                {
                    SapOrderReference = g.Key.SapOrderReference,
                    CustomerName = g.Key.CustomerName,
                    OrderNumber = g.Key.OrderNumber,
                    PositionName = g.Key.PositionName,
                    PayplanType = g.Key.PayplanType,
                    OrderListVal = g.Sum(x => x.dca.AllocationList ?? 0),
                    OrderSaleVal = g.Sum(x => x.dca.AllocationValue ?? 0),
                    OrderDiscountPercent = g.Key.OrderDiscountPercent,
                    ServiceType = g.Key.ServiceType,
                    ServiceDiscountPercent = g.Key.ServiceDiscountPercent,
                    OrderType = g.Key.OrderType,
                    CustomerType = g.Key.CustomerType
                }
            ).ToListAsync();

            // 4. Unit Summary
            var unitSummary = await (
                from dca in _context.DataCreditAllocations
                join cat in _context.ConfigAllocationTypes on dca.AllocationTypeId equals cat.AllocationTypeId
                join doh in _context.DataOrderHeaders on dca.OrderId equals doh.OrderRowId
                join doi in _context.DataOrderItems on new { OrderId = dca.OrderId, ItemId = dca.ItemId } equals new { OrderId = doi.OrderRowId, ItemId = doi.OrderItemId }
                where (dca.AllocationSource == "ORDER_ITEM" || dca.AllocationSource == "PRODUCT_ITEM")
                    && cat.AllocationType == "UNITS"
                    && cat.AllocationTypeId != 23
                    && dca.EmployeeId == empid
                    && dca.PeriodMonth == month
                    && dca.PeriodYear == year
                select new UnitSummaryDto
                {
                    OrderNumber = doh.OrderNumber,
                    LineNumber = doi.LineNumber,
                    ProductCode = doi.ProductCode,
                    ProductDesc = doi.ProductDesc,
                    AllocationDescription = cat.AllocationDescription,
                    AllocationValue = (int)(dca.AllocationValue ?? 0),
                    ExcludeFromCalcs = dca.ExcludeFromCalcs == "0" ? "NO" : "YES"
                }
            ).ToListAsync();

            // 5. Order Allocation Totals
            var orderAllocTotals = await (
                from dca in _context.DataCreditAllocations
                join doh in _context.DataOrderHeaders on dca.OrderId equals doh.OrderRowId
                join doi in _context.DataOrderItems on new { OrderId = dca.OrderId, ItemId = dca.ItemId } equals new { OrderId = doi.OrderRowId, ItemId = doi.OrderItemId }
                join dop in _context.DataOrderPositions on new { OrderId = dca.OrderId, PositionId = dca.PositionId } equals new { OrderId = dop.OrderRowId, PositionId = dop.PositionRowId }
                where dca.AllocationTypeId == 1
                    && dca.AllocationSource != "ROLLUP"
                    && dca.EmployeeId == empid
                    && dca.PeriodMonth == month
                    && dca.PeriodYear == year
                group new { dca, doh, dop } by new
                {
                    PrimaryFlag = dop.PositionRowId == doh.PrPostnId ? "1" : "0",
                    doh.OrderNumber,
                    dop.PositionName,
                    dop.PayplanType,
                    doh.OrderDiscountPercent,
                    doh.ServiceDiscountPercent,
                    dop.AllocationPercentage,
                    dca.ExcludeFromCalcs
                } into g
                select new OrderAllocationTotalDto
                {
                    PrimaryFlag = g.Key.PrimaryFlag,
                    OrderNumber = g.Key.OrderNumber,
                    PositionName = g.Key.PositionName,
                    PayplanType = g.Key.PayplanType,
                    OrderListVal = g.Sum(x => x.doh.OrderListVal ?? 0),
                    OrderSaleVal = g.Sum(x => x.doh.OrderSaleVal ?? 0),
                    OrderDiscountPercent = g.Key.OrderDiscountPercent,
                    ServiceDiscountPercent = g.Key.ServiceDiscountPercent,
                    AllocationPercentage = g.Key.AllocationPercentage,
                    AllocationList = g.Sum(x => x.dca.AllocationList ?? 0),
                    AllocationValue = g.Sum(x => x.dca.AllocationValue ?? 0),
                    ExcludeFromCalcs = g.Key.ExcludeFromCalcs == "0" ? "NO" : "YES"
                }
            ).ToListAsync();

            // 6. Allocation Totals
            var allocationTotals = await (
                from dca in _context.DataCreditAllocations
                join cat in _context.ConfigAllocationTypes on dca.AllocationTypeId equals cat.AllocationTypeId
                where dca.ExcludeFromCalcs == "0"
                    && dca.EmployeeId == empid
                    && dca.PeriodMonth == month
                    && dca.PeriodYear == year
                group new { dca, cat } by new
                {
                    cat.AllocationTypeId,
                    cat.AllocationType,
                    cat.AllocationDescription
                } into g
                select new AllocationTotalDto
                {
                    AllocationTypeId = g.Key.AllocationTypeId,
                    AllocationType = g.Key.AllocationType,
                    AllocationDescription = g.Key.AllocationDescription,
                    AllocationValue = g.Sum(x => x.dca.AllocationValue ?? 0)
                }
            ).ToListAsync();

            // 7. Payment Details
            var paymentDetails = await (
                from dcp in _context.DataCommissionPayments
                join doh in _context.DataOrderHeaders on dcp.OrderId equals doh.OrderRowId into dohGroup
                from doh in dohGroup.DefaultIfEmpty()
                where dcp.EmployeeId == empid
                    && dcp.PeriodMonth == month
                    && dcp.PeriodYear == year
                select new EmpPaymentDetailDto
                {
                    OrderNumber = doh != null ? doh.OrderNumber : "X",
                    CustomerName = doh != null ? doh.CustomerName : null,
                    PaymentDescription = dcp.PaymentDescription,
                    PaymentRate = dcp.PaymentRate,
                    PaymentValue = dcp.PaymentValue ?? 0,
                    PaymentWithheld = dcp.PaymentWitheld ? "YES" : "NO"
                }
            ).ToListAsync();

            // 8. Payment Total
            var paymentTotal = await _context.DataCommissionPayments
                .Where(dcp => dcp.EmployeeId == empid
                    && dcp.PeriodMonth == month
                    && dcp.PeriodYear == year
                    && !dcp.PaymentWitheld)
                .SumAsync(dcp => dcp.PaymentValue ?? 0);

            // 9. Adjustments Summary (UNION of 3 queries)
            var adj1 = await (
                from dca in _context.DataCreditAllocations
                join doh in _context.DataOrderHeaders on dca.OrderId equals doh.OrderRowId into dohGroup
                from doh in dohGroup.DefaultIfEmpty()
                where dca.AllocationSource == "ADJUSTMENT"
                    && dca.EmployeeId == empid
                    && dca.PeriodMonth == month
                    && dca.PeriodYear == year
                join cat in _context.ConfigAllocationTypes on dca.AllocationTypeId equals cat.AllocationTypeId into catGroup
                from cat in catGroup.DefaultIfEmpty()
                select new AdjustmentSummaryDto
                {
                    OrderNumber = doh != null ? doh.OrderNumber : "X",
                    CustomerName = doh != null ? doh.CustomerName : null,
                    AdjustmentReason = cat != null ? cat.AllocationDescription : null,
                    AdjustmentType = "Credit Allocation",
                    AllocationList = dca.AllocationList ?? 0,
                    AllocationValue = dca.AllocationValue ?? 0,
                    PaymentValue = 0
                }
            ).ToListAsync();

            var adj2 = await (
                from dcp in _context.DataCommissionPayments
                join doh in _context.DataOrderHeaders on dcp.OrderId equals doh.OrderRowId into dohGroup
                from doh in dohGroup.DefaultIfEmpty()
                where dcp.PaymentSource == "ADJUSTMENT"
                    && dcp.EmployeeId == empid
                    && dcp.PeriodMonth == month
                    && dcp.PeriodYear == year
                select new AdjustmentSummaryDto
                {
                    OrderNumber = doh != null ? doh.OrderNumber : "X",
                    CustomerName = doh != null ? doh.CustomerName : null,
                    AdjustmentReason = dcp.PaymentDescription,
                    AdjustmentType = "Commission Payment",
                    AllocationList = 0,
                    AllocationValue = 0,
                    PaymentValue = dcp.PaymentValue ?? 0
                }
            ).ToListAsync();

            var adj3 = await (
                from dca in _context.DataCreditAllocations
                join doh in _context.DataOrderHeaders on dca.OrderId equals doh.OrderRowId into dohGroup
                from doh in dohGroup.DefaultIfEmpty()
                where (dca.AllocationSource == "PRODUCT_ITEM" || dca.AllocationSource == "ROLLUP")
                    && dca.AdjustmentId != null
                    && dca.EmployeeId == empid
                    && dca.PeriodMonth == month
                    && dca.PeriodYear == year
                join cat in _context.ConfigAllocationTypes on dca.AllocationTypeId equals cat.AllocationTypeId into catGroup
                from cat in catGroup.DefaultIfEmpty()
                select new AdjustmentSummaryDto
                {
                    OrderNumber = doh != null ? doh.OrderNumber : "X",
                    CustomerName = doh != null ? doh.CustomerName : null,
                    AdjustmentReason = cat != null ? cat.AllocationDescription : null,
                    AdjustmentType = "Credit Allocation (Product/Rollup)",
                    AllocationList = dca.AllocationList ?? 0,
                    AllocationValue = dca.AllocationValue ?? 0,
                    PaymentValue = 0
                }
            ).ToListAsync();

            var adjustments = adj1.Concat(adj2).Concat(adj3).ToList();

            // 10. Order Lines (nested)
            var orderLinesRaw = await (
                from dca in _context.DataCreditAllocations
                join cat in _context.ConfigAllocationTypes on dca.AllocationTypeId equals cat.AllocationTypeId
                join doh in _context.DataOrderHeaders on dca.OrderId equals doh.OrderRowId
                where dca.AllocationSource != "ROLLUP"
                    && dca.AllocationTypeId == 1
                    && dca.EmployeeId == empid
                    && dca.PeriodMonth == month
                    && dca.PeriodYear == year
                select new
                {
                    doh.OrderNumber,
                    cat.AllocationDescription,
                    dca.AllocationValue,
                    dca.ExcludeFromCalcs
                }
            ).ToListAsync();

            var orderGroups = orderLinesRaw
                .GroupBy(x => x.OrderNumber)
                .Select(g => new OrderLineGroupDto
                {
                    OrderNumber = g.Key,
                    TotalAllocationValue = g.Sum(x => x.AllocationValue ?? 0),
                    ExcludeFromCalcs = g.Any(x => x.ExcludeFromCalcs != "0") ? "YES" : "NO",
                    Lines = g.GroupBy(x => x.AllocationDescription)
                        .Select(lg => new OrderLineDetailDto
                        {
                            AllocationDescription = lg.Key,
                            AllocationValue = lg.Sum(x => x.AllocationValue ?? 0)
                        }).ToList()
                }).ToList();

            return new EmployeeSummaryDto
            {
                EmployeeDetails = empDetail,
                AllocatedOrders = allocatedOrders,
                UnitSummary = unitSummary,
                OrderAllocationTotals = orderAllocTotals,
                AllocationTotals = allocationTotals,
                PaymentDetails = paymentDetails,
                PaymentTotal = paymentTotal,
                Adjustments = adjustments,
                OrderLines = orderGroups
            };
        }
    }
}

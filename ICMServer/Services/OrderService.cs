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

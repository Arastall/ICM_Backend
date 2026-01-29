/*using ICMServer.DBContext;
using ICMServer.Helpers;
using ICMServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ICMServer.Services
{
    public interface IRevenueTotalsService
    {
        Task RecordRevenueTotalsAsync(string orderId);
    }

    public class RevenueTotalsService : IRevenueTotalsService
    {
        private ICMDBContext _context;
        private readonly ISystemParametersHelper _sysParams;
        private readonly ILogger<RevenueTotalsService> _logger;
        private readonly IServiceProvider _sp;

        public RevenueTotalsService(
            ISystemParametersHelper sysParams,
            ILogger<RevenueTotalsService> logger, IServiceProvider sp)
        {
            _sysParams = sysParams;
            _logger = logger;
            _sp = sp;

            using var scope = _sp.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<ICMDBContext>();
        }

        public async Task RecordRevenueTotalsAsync(string orderId)
        {
            try
            {
                var periodYear = await _sysParams.GetSysParamAsync("PERIOD_YEAR");
                var periodMonth = await _sysParams.GetSysParamAsync("PERIOD_MONTH");

                _logger.LogDebug("Adding total revenues for employees paid for order {OrderId}", orderId);

                // Get distinct employees
                var employeeIds = await _context.DataCreditAllocations
                    .Where(a => a.OrderId == orderId
                        && a.PeriodMonth == periodMonth
                        && a.PeriodYear == periodYear)
                    .Select(a => a.EmployeeId)
                    .Distinct()
                    .ToListAsync();

                foreach (var employeeId in employeeIds)
                {
                    // Get distinct positions for this employee
                    var positionIds = await _context.DataCreditAllocations
                        .Where(a => a.OrderId == orderId
                            && a.EmployeeId == employeeId
                            && a.PeriodMonth == periodMonth
                            && a.PeriodYear == periodYear)
                        .Select(a => a.PositionId)
                        .Distinct()
                        .ToListAsync();

                    foreach (var positionId in positionIds)
                    {
                        // Get distinct allocation IDs for this position
                        var allocIds = await _context.DataCreditAllocations
                            .Where(a => a.OrderId == orderId
                                && a.EmployeeId == employeeId
                                && a.PositionId == positionId
                                && a.PeriodMonth == periodMonth
                                && a.PeriodYear == periodYear)
                            .Select(a => a.AllocationTypeId)
                            .Distinct()
                            .ToListAsync();

                        foreach (var allocId in allocIds)
                        {
                            // Calculate total revenue for this allocation type
                            var revenueTotal = await _context.DataCreditAllocations
                                .Where(a => a.EmployeeId == employeeId
                                    && a.PositionId == positionId
                                    && a.PeriodYear == periodYear
                                    && a.AllocationTypeId == allocId
                                    && a.AdjustmentId == null)
                                .SumAsync(a => (decimal?)a.AllocationValue) ?? 0;

                            _logger.LogDebug("EmployeeId: {EmployeeId} - PositionID: {PositionId}", employeeId, positionId);
                            _logger.LogDebug("AllocID: {AllocId} - Total Revenues: {Total}", allocId, revenueTotal);

                            // Insert revenue record
                            _context.DataRevenueAgainstOrders.Add(new DataRevenueAgainstOrder
                            {
                                OrderId = orderId,
                                EmployeeId = employeeId,
                                RevenueTotal = revenueTotal,
                                RevenueId = allocId,
                                PeriodMonth = periodMonth,
                                PeriodYear = periodYear,
                                TimeStamp = DateTime.Now
                            });
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error recording revenue totals for order {OrderId}", orderId);
                throw;
            }
        }

    }
}*/
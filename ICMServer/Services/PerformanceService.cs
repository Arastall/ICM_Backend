
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ICMServer.Helpers;
using ICMServer.DBContext;

namespace ICMServer.Services
{
    public interface IPerformanceService
    {
        Task<decimal> GetPerformanceAsync(int targetAllocId, string orderId, string employeeId, string payplan);
        Task<decimal> CheckHigherRatesAsync(string orderId, string employeeId, string payPlan, int allocId, decimal revenuePercentage);
    }

    public class PerformanceService : IPerformanceService
    {
        private ICMDBContext _context;
        private readonly IPeriodContext _periodContext;
        private readonly ILogger<PerformanceService> _logger;
        private readonly IServiceProvider _sp;

        public PerformanceService(
            IPeriodContext periodContext,
            ILogger<PerformanceService> logger, IServiceProvider sp)
        {
            _periodContext = periodContext;
            _logger = logger;
            _sp = sp;

            using var scope = _sp.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<ICMDBContext>();
        }

        public async Task<decimal> GetPerformanceAsync(int targetAllocId, string orderId, string employeeId, string payplan)
        {
            try
            {
                var periodYear = await _periodContext.GetPeriodYearAsync();
                var periodMonth = await _periodContext.GetPeriodMonthAsync();

                // Get total revenue for this employee and allocation type
                var revenueTotalValue = await _context.DataCreditAllocations
                    .Where(a => a.EmployeeId == employeeId
                        && a.Payplan == payplan
                        && a.PeriodMonth.CompareTo(periodMonth) <= 0
                        && a.PeriodYear == periodYear
                        && a.AllocationTypeId == targetAllocId)
                    .SumAsync(a => (decimal?)a.AllocationValue) ?? 0;

                // Get target value for this payplan and allocation type
                var targetValue = await _context.DataDefaultTargets
                    .Where(t => t.PayPlanType == payplan
                        && t.FinYear == periodYear
                        && t.AllocationTypeId == targetAllocId
                        && t.EmployeeId == employeeId)
                    .Select(t => (decimal?)t.TargetValue)
                    .FirstOrDefaultAsync();

                // If no employee-specific target, use generic payplan target
                if (targetValue == null)
                {
                    targetValue = await _context.DataDefaultTargets
                        .Where(t => t.PayPlanType == payplan
                            && t.FinYear == periodYear
                            && t.AllocationTypeId == targetAllocId
                            && t.EmployeeId == "X")
                        .Select(t => (decimal?)t.TargetValue)
                        .FirstOrDefaultAsync();
                }

                if (targetValue == null || targetValue == 0)
                {
                    _logger.LogWarning("No target found for payplan {Payplan}", payplan);
                    return 1;
                }

                var performance = (revenueTotalValue / targetValue.Value) * 100;
                _logger.LogDebug("Performance: {Revenue} / {Target} = {Performance}%",
                    revenueTotalValue, targetValue, performance);

                return performance;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting performance");
                throw;
            }
        }

        public async Task<decimal> CheckHigherRatesAsync(string orderId, string employeeId, string payPlan, int allocId, decimal revenuePercentage)
        {
            try
            {
                decimal result = revenuePercentage;
                decimal? arrPercentage = null;
                decimal? mailPercentage = null;
                decimal? plsPercentage = null;

                _logger.LogDebug("Checking higher rates for Payplan {PayPlan} - AllocId {AllocId}", payPlan, allocId);

                var eligiblePayplans = new[] { "1K", "2K", "3K", "3S", "2P", "3P", "6K", "5P", "6P", "MB", "MP", "PM", "ME", "FM", "FS" };

                if (eligiblePayplans.Contains(payPlan))
                {
                    // Get ARR performance (267)
                    arrPercentage = await GetPerformanceAsync(267, orderId, employeeId, payPlan);

                    if (new[] { "FM", "FS" }.Contains(payPlan))
                    {
                        // Get Mail performance (264)
                        mailPercentage = await GetPerformanceAsync(264, orderId, employeeId, payPlan);
                    }

                    if (payPlan == "ME")
                    {
                        if (allocId != 270)
                        {
                            // Get PLS performance (270)
                            plsPercentage = await GetPerformanceAsync(270, orderId, employeeId, payPlan);
                        }
                        if (allocId != 264)
                        {
                            // Get Mail performance (264)
                            mailPercentage = await GetPerformanceAsync(264, orderId, employeeId, payPlan);
                        }
                    }
                }

                // Apply business rules based on allocId and payplan
                if (allocId == 264) // Mail
                {
                    if (new[] { "1K", "2K", "3K", "3S", "2P", "3P", "6K", "5P", "6P" }.Contains(payPlan))
                    {
                        if (revenuePercentage >= 100 && arrPercentage <= 80)
                        {
                            result = 99.9m;
                            _logger.LogDebug("Performance capped - ARR not reached. Mail: {Mail}% - ARR: {Arr}%",
                                revenuePercentage, arrPercentage);
                        }
                    }
                    else if (new[] { "MB", "MP", "PM" }.Contains(payPlan))
                    {
                        if (revenuePercentage >= 105 && arrPercentage <= 80)
                        {
                            result = 104.99m;
                            _logger.LogDebug("Performance capped - ARR not reached. Mail: {Mail}% - ARR: {Arr}%",
                                revenuePercentage, arrPercentage);
                        }
                    }
                    else if (payPlan == "ME")
                    {
                        if (revenuePercentage >= 100 && (arrPercentage <= 80 || plsPercentage <= 80))
                        {
                            result = 99.9m;
                            _logger.LogDebug("Performance capped - PLS or ARR not reached. Mail: {Mail}% - PLS: {Pls}% - ARR: {Arr}%",
                                revenuePercentage, plsPercentage, arrPercentage);
                        }
                    }
                }
                else if (allocId == 270) // PLS
                {
                    if (payPlan == "FM")
                    {
                        if (revenuePercentage >= 100 && mailPercentage <= 80)
                        {
                            result = 99.9m;
                            _logger.LogDebug("Performance capped - Mail not reached. PLS: {Pls}% - Mail: {Mail}%",
                                revenuePercentage, mailPercentage);
                        }
                    }
                    else if (payPlan == "FS")
                    {
                        if (revenuePercentage >= 100 && mailPercentage <= 100)
                        {
                            result = 99.9m;
                            _logger.LogDebug("Performance capped - Mail not reached. PLS: {Pls}% - Mail: {Mail}%",
                                revenuePercentage, mailPercentage);
                        }
                    }
                    else if (payPlan == "ME")
                    {
                        if (revenuePercentage >= 100 && (arrPercentage <= 80 || mailPercentage <= 80))
                        {
                            result = 99.9m;
                            _logger.LogDebug("Performance capped - Mail or ARR not reached. PLS: {Pls}% - Mail: {Mail}% - ARR: {Arr}%",
                                revenuePercentage, mailPercentage, arrPercentage);
                        }
                    }
                }
                else if (allocId == 267 && payPlan == "ME") // ARR
                {
                    if (revenuePercentage >= 100 && (plsPercentage <= 80 || mailPercentage <= 80))
                    {
                        result = 99.9m;
                        _logger.LogDebug("Performance capped - Mail or PLS not reached. ARR: {Arr}% - Mail: {Mail}% - PLS: {Pls}%",
                            revenuePercentage, mailPercentage, plsPercentage);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking higher rates");
                throw;
            }
        }
    }
}

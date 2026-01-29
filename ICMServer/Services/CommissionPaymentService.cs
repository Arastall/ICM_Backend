using ICMServer.DBContext;
using ICMServer.Helpers;
using ICMServer.Models;
using ICMServer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ICMServer.Services
{
    public interface ICommissionPaymentService
    {
        Task ProcessCommissionPaymentAsync(string orderId);
    }

    public class CommissionPaymentService : ICommissionPaymentService
    {
        private ICMDBContext _context;
        private readonly ISystemParametersHelper _sysParams;
        private readonly IRevenueCalculationService _revenueCalculation;
        private readonly ILogger<CommissionPaymentService> _logger;
        private readonly IServiceProvider _sp;
        private readonly IPeriodContext _periodContext;

        public CommissionPaymentService(
            ISystemParametersHelper sysParams,
            IRevenueCalculationService revenueCalculation, IPeriodContext periodContext,
            ILogger<CommissionPaymentService> logger, IServiceProvider sp)
        {
            _sysParams = sysParams;
            _revenueCalculation = revenueCalculation;
            _periodContext = periodContext;
            _logger = logger;
            _sp = sp;
            using var scope = _sp.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<ICMDBContext>();
        }

        public async Task ProcessCommissionPaymentAsync(string orderId)
        {
            try
            {
                _logger.LogDebug("COMMISSIONS PAYMENT - Started");

                var periodYear = await _periodContext.GetPeriodYearAsync();
                var periodMonth = await _periodContext.GetPeriodMonthAsync();

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
                     var positionsData = await _context.DataCreditAllocations
                        .Where(a => a.OrderId == orderId
                            && a.EmployeeId == employeeId
                            && a.PeriodMonth == periodMonth
                            && a.PeriodYear == periodYear)
                        .Select(a => new { a.Payplan, a.PositionId })
                        .Distinct()
                        .ToListAsync();

                    foreach (var pos in positionsData)
                    {
                        // Check if payplan is configured
                        var payplanExists = await _context.ConfigProcessingTypes.AnyAsync(c => c.PeriodYear == periodYear && c.Payplan == pos.Payplan);

                        if (!payplanExists)
                        {
                            _logger.LogWarning("Payplan {Payplan} not set in CONFIG_PROCESSING_TYPE", pos.Payplan);

                            // Track unset payplan
                            var unsetExists = await _context.PayplanUnsets
                                .AnyAsync(p => p.Payplan == pos.Payplan && p.Orderid == orderId);

                            if (!unsetExists)
                            {
                                _context.PayplanUnsets.Add(new PayplanUnset
                                {
                                    Payplan = pos.Payplan,
                                    Orderid = orderId
                                });
                                await _context.SaveChangesAsync();
                            }
                            continue;
                        }

                        var employeeName = await _context.DataEmployees
                            .Where(e => e.RowId == employeeId)
                            .Select(e => e.FstName + " " + e.LastName)
                            .FirstOrDefaultAsync();

                        _logger.LogDebug("{EmployeeName} (EmployeeID {EmployeeId}) - Payplan {Payplan}",
                            employeeName, employeeId, pos.Payplan);

                        // Get allocation IDs
                        List<int?> allocationIds;

                        if (pos.Payplan == "EN")
                        {
                            allocationIds = await _context.DataCreditAllocations
                                .Where(a => a.OrderId == orderId
                                    && a.EmployeeId == employeeId
                                    && a.AllocationTypeId == 277)
                                .Select(a => a.AllocationTypeId)
                                .Distinct()
                                .ToListAsync();
                        }
                        else
                        {
                            var excludedTypes = new[] { 1, 218, 219, 267, 268, 269, 277 };
                            allocationIds = await _context.DataCreditAllocations
                                .Where(a => a.OrderId == orderId
                                    && a.EmployeeId == employeeId
                                    && a.AdjustmentId == null
                                    && !excludedTypes.Contains(a.AllocationTypeId.Value))
                                .Select(a => a.AllocationTypeId)
                                .Distinct()
                                .ToListAsync();
                        }

                        // Process each allocation type
                        foreach (var allocId in allocationIds)
                        {
                            var rateValue = await _revenueCalculation.GetRevenueRateValueFromAllocIdAsync(allocId.Value, orderId, pos.Payplan, employeeId);

                            if (rateValue.HasValue)
                            {
                                _logger.LogDebug("Payment Rate {Rate} for payplan {payplan} and KPI {AllocId}",
                                    rateValue, pos.Payplan, allocId);

                                var allocSource = await _context.DataCreditAllocations
                                    .Where(a => a.OrderId == orderId
                                        && a.EmployeeId == employeeId
                                        && a.AllocationTypeId == allocId)
                                    .Select(a => a.AllocationSource)
                                    .FirstOrDefaultAsync();

                                await SetPaymentAsync(orderId, pos.PositionId, rateValue.Value, allocSource, allocId.Value, employeeId, pos.Payplan);
                            }
                        }
                    }
                }

                await SetCommissionProcessedFlagAsync(orderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing commission payment for order {OrderId}", orderId);
                throw;
            }
        }

        private async Task SetPaymentAsync(string orderId, string positionId, decimal rateValue,
            string paymentSource, int allocId, string employeeId, string payplan)
        {
            try
            {
                var periodYear = await _periodContext.GetPeriodYearAsync();

                var paymentDescription = await _context.ConfigAllocationTypes
                    .Where(a => a.AllocationTypeId == allocId)
                    .Select(a => a.AllocationDescription)
                    .FirstOrDefaultAsync();

                var procType = await _context.ConfigProcessingTypes
                    .Where(p => p.Payplan == payplan && p.PeriodYear == periodYear)
                    .Select(p => p.ProcessingType)
                    .FirstOrDefaultAsync();

                if (procType == "SM")
                {
                    paymentDescription = "Manager Commission - " + paymentDescription;
                }
                else if (payplan == "F1")
                {
                    paymentDescription = "F1 Commission - " + paymentDescription;
                }

                _logger.LogDebug("Employee Id: {EmployeeId} - Payplan: {Payplan}", employeeId, payplan);

                var totalRevenueValue = await _context.DataCreditAllocations
                    .Where(a => a.OrderId == orderId
                        && a.PositionId == positionId
                        && a.EmployeeId == employeeId
                        && a.AllocationTypeId == allocId
                        && a.AdjustmentId == null)
                    .SumAsync(a => (decimal?)a.AllocationValue) ?? 0;

                var paymentValue = totalRevenueValue * (rateValue / 100);

                // Check if this is an engineer payplan
                var engineerPayplans = await _context.ConfigSystemParameters
                    .Where(p => p.ParameterName == "ENGINEER")
                    .Select(p => p.ParameterValue)
                    .ToListAsync();

                if (engineerPayplans.Contains(payplan))
                {
                    paymentDescription = "Engineer Commission";
                    var engineerCap = decimal.Parse(await _sysParams.GetSysParamAsync("ENGINEER_PAY_CAP"));

                    if (paymentValue > engineerCap)
                    {
                        paymentValue = engineerCap;
                        paymentDescription += $" (capped at {engineerCap})";
                    }

                    if (paymentValue < -engineerCap)
                    {
                        paymentValue = -engineerCap;
                    }
                }

                paymentValue = Math.Round(paymentValue, 2);

                _logger.LogDebug("What we pay: {Payment} ({Rate}% of {Total})",
                    paymentValue, rateValue, totalRevenueValue);

                _context.DataCommissionPayments.Add(new DataCommissionPayment
                {
                    PositionId = positionId,
                    EmployeeId = employeeId,
                    PaymentSource = paymentSource,
                    PaymentDescription = paymentDescription,
                    OrderId = orderId,
                    EnhancementId = null,
                    PaymentRate = rateValue,
                    PaymentValue = paymentValue,
                    PayplanRateId = null
                });

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting payment");
                throw;
            }
        }

        private async Task SetCommissionProcessedFlagAsync(string orderId)
        {
            try
            {
                var processHistory = await _context.DataOrderProcessHistories.FirstOrDefaultAsync(h => h.RowId == orderId);

                if (processHistory != null)
                {
                    processHistory.CommissionProcessed = true;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting commission processed flag");
                throw;
            }
        }
    }
}
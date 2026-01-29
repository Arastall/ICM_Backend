using ICMServer.DBContext;
using ICMServer.Helpers;
using ICMServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ICMServer.Services
{
    public interface IRevenueCalculationService
    {
        Task CalculateRevenueForProductAndRepAsync(string orderId, string itemId, string positionRowId);
        Task<decimal?> GetRevenueRateValueFromAllocIdAsync(int allocId, string rowId, string payPlanType, string employeeId);
    }

    public class RevenueCalculationService : IRevenueCalculationService
    {
        private ICMDBContext _context;
        private readonly ISystemParametersHelper _sysParams;
        private readonly IProductValidationService _productValidation;
        private readonly IPerformanceService _performanceService;
        private readonly IRollupAllocationService _rollupService;
        private readonly ILogger<RevenueCalculationService> _logger;
        private readonly IPeriodContext _periodContext;
        private readonly IServiceProvider _sp;

        public RevenueCalculationService(
            ISystemParametersHelper sysParams,
            IPeriodContext periodContext,
            IProductValidationService productValidation,
            IPerformanceService performanceService,
            IRollupAllocationService rollupService,
            ILogger<RevenueCalculationService> logger, IServiceProvider sp)
        {
            _sysParams = sysParams;
            _productValidation = productValidation;
            _performanceService = performanceService;
            _rollupService = rollupService;
            _periodContext = periodContext;
            _logger = logger;
            _sp = sp;

        }

        public async Task CalculateRevenueForProductAndRepAsync(string orderId, string itemId, string positionRowId)
        {
            try
            {
                using var scope = _sp.CreateScope();
                _context = scope.ServiceProvider.GetRequiredService<ICMDBContext>();

                var periodYear = await _periodContext.GetPeriodYearAsync();
                var periodMonth = await _periodContext.GetPeriodMonthAsync();

                // Get order data
                var orderHeader = await _context.DataOrderHeaders
                    .Where(o => o.OrderRowId == orderId)
                    .Select(o => new
                    {
                        o.PromotionCode,
                        o.PrPostnId,
                        o.CustomerType
                    })
                    .FirstOrDefaultAsync();

                // Get order item data
                var orderItem = await _context.DataOrderItems
                    .Where(i => i.OrderItemId == itemId)
                    .Select(i => new
                    {
                        i.ListValue,
                        i.SaleValue,
                        i.Quantity
                    })
                    .FirstOrDefaultAsync();

                // Get position allocation data
                var position = await _context.DataOrderPositions
                    .Where(p => p.PositionRowId == positionRowId && p.OrderRowId == orderId)
                    .Select(p => new
                    {
                        p.EmployeeRowId,
                        p.AllocationPercentage,
                        p.PayplanType
                    })
                    .FirstOrDefaultAsync();

                if (position == null)
                {
                    _logger.LogWarning("Position {PositionRowId} not found for order {OrderId}", positionRowId, orderId);
                    return;
                }

                _logger.LogDebug("Calculate revenue for position {PositionId} ({PayPlan} {Percentage}%)",
                    positionRowId, position.PayplanType, position.AllocationPercentage);

                // Get allocation IDs based on whether it's an engineer
                bool isEngineer = position.PayplanType == "EN";
                var allocsId = await _productValidation.GetProductAllocationIdsAsync(orderId, itemId, isEngineer);

                if (string.IsNullOrEmpty(allocsId))
                {
                    _logger.LogDebug("No allocation IDs found for item {ItemId}", itemId);
                    return;
                }

                _logger.LogDebug("BUCKET(S) DETECTED: {AllocsId} (Item {ItemId})", allocsId, itemId);

                // Calculate allocation values
                decimal allocationList = (orderItem.ListValue ?? 0) * (orderItem.Quantity ?? 0) * ((position.AllocationPercentage ?? 0) / 100);
                decimal allocationValue = (orderItem.SaleValue ?? 0) * (orderItem.Quantity ?? 0) * ((position.AllocationPercentage ?? 0) / 100);

                _logger.LogDebug("Revenue - Allocation list: {AllocationList}", allocationList);
                _logger.LogDebug("Revenue - Allocation value: {AllocationValue}", allocationValue);

                // Check for uplifted rate
                /*if (!string.IsNullOrEmpty(orderHeader.PromotionCode) && orderHeader.PromotionCode.Length >= 3)
                {
                    var promoCode = orderHeader.PromotionCode.Substring(0, 3);
                    var uplift = await _context.ConfigPromoUplifts
                        .Where(p => p.PeriodYear == periodYear
                            && p.PeriodMonth == periodMonth
                            && p.PromoCode == promoCode)
                        .Select(p => (decimal?)p.Uplift)
                        .FirstOrDefaultAsync();

                    if (uplift.HasValue)
                    {
                        allocationValue = allocationValue + ((allocationValue / 100) * uplift.Value);
                        _logger.LogDebug("Applied promo uplift: {Uplift}%", uplift);
                    }
                }*/

                // Process each allocation ID
                var allocIds = allocsId.Split(',').Select(int.Parse).ToList();

                foreach (var allocId in allocIds)
                {
                    // Check if we should process this allocation based on customer type
                    bool shouldProcess = false;

                    if (allocId != 271)
                    {
                        shouldProcess = true;
                    }
                    else if (allocId == 271)
                    {
                        var competitorCustomerTypes = new[] { "Non User", "Competitor", "Comp User", "Comp Special", "Non User Special" };
                        var engineerCompetitorTypes = new[] { "Non User", "Competitor", "Comp User", "User Additional", "Comp Special", "Non User Special" };

                        if (position.PayplanType != "EN" && competitorCustomerTypes.Contains(orderHeader.CustomerType))
                        {
                            shouldProcess = true;
                        }
                        else if (position.PayplanType == "EN" && engineerCompetitorTypes.Contains(orderHeader.CustomerType))
                        {
                            shouldProcess = true;
                        }
                    }

                    if (shouldProcess)
                    {
                        _logger.LogDebug("Current Alloc ID: {AllocId}", allocId);
                        _logger.LogDebug("Revenue added for employee: {EmployeeId}", position.EmployeeRowId);

                        var creditAllocation = new DataCreditAllocation
                        {
                            PositionId = positionRowId,
                            EmployeeId = position.EmployeeRowId,
                            ChildPositionId = null,
                            AllocationSource = "ORDER_ITEM",
                            OrderId = orderId,
                            ItemId = itemId,
                            AdjustmentId = null,
                            AllocationTypeId = allocId,
                            AllocationList = Math.Round(allocationList, 2),
                            AllocationValue = Math.Round(allocationValue, 2),
                            ExcludeFromCalcs = "0",
                            RollupProcessed = "N",
                            Payplan = position.PayplanType,
                            PeriodYear = periodYear,
                            PeriodMonth = periodMonth
                        };

                        _context.DataCreditAllocations.Add(creditAllocation);
                        await _context.SaveChangesAsync();

                        var lastCreditAllocationId = creditAllocation.DataCreditAllocationId;

                        // Trigger rollup for this allocation
                        await _rollupService.RollupAllocationAsync(orderId, lastCreditAllocationId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating revenue for product and rep");
                throw;
            }
        }

        public async Task<decimal?> GetRevenueRateValueFromAllocIdAsync(int allocId, string rowId, string payPlanType, string employeeId)
        {
            try
            {
                using var scope = _sp.CreateScope();
                _context = scope.ServiceProvider.GetRequiredService<ICMDBContext>();

                var periodYear = await _periodContext.GetPeriodYearAsync();
                var periodMonth = await _periodContext.GetPeriodMonthAsync();

                _logger.LogDebug("Current Bucket: {AllocId}", allocId);

                decimal? rateValue = null;

                if (payPlanType == "EN") // Engineer
                {
                    var orderHeader = await _context.DataOrderHeaders
                        .Where(o => o.OrderRowId == rowId)
                        .Select(o => new
                        {
                            o.OrderNumber,
                            o.OrderType,
                            o.CustomerType
                        })
                        .FirstOrDefaultAsync();

                    string customerTypeCode;
                    if (orderHeader.OrderNumber.StartsWith("0001"))
                    {
                        customerTypeCode = "XXX";
                    }
                    else if (orderHeader.OrderType == "Supplies Order" ||
                             new[] { "Competitor", "Non User", "Comp User", "User Additional" }.Contains(orderHeader.CustomerType))
                    {
                        customerTypeCode = "XX";
                    }
                    else
                    {
                        customerTypeCode = "X";
                    }

                    _logger.LogDebug("Engineer detected with code: {Code}", customerTypeCode);

                    rateValue = await _context.DataPerformanceAgainstTargetMatrices
                        .Where(m => m.Payplan == payPlanType
                            && m.EmployeeId == customerTypeCode
                            && m.PeriodYear == Convert.ToInt32(periodYear)
                            && int.Parse(periodMonth) >= m.PeriodMonthStart
                            && int.Parse(periodMonth) <= m.PeriodMonthEnd
                            && m.AllocationId == allocId)
                        .Select(m => (decimal?)m.CommissionValue)
                        .FirstOrDefaultAsync();
                }
                else
                {
                    // Get performance
                    var revenuePercentage = await _performanceService.GetPerformanceAsync(allocId, rowId, employeeId, payPlanType);

                    if (revenuePercentage > 100)
                    {
                        _logger.LogDebug("Performance over 100%, checking higher rates...");
                        revenuePercentage = await _performanceService.CheckHigherRatesAsync(rowId, employeeId, payPlanType, allocId, revenuePercentage);
                    }

                    _logger.LogDebug("Get rate in DPATM Table from empId: {Performance}% Emp: {EmployeeId}",
                        revenuePercentage, employeeId);

                    // Try to get employee-specific rate
                    rateValue = await _context.DataPerformanceAgainstTargetMatrices
                        .Where(m => m.Payplan == payPlanType
                            && m.EmployeeId == employeeId
                            && m.PeriodYear == Convert.ToInt32(periodYear)
                            && int.Parse(periodMonth) >= m.PeriodMonthStart
                            && int.Parse(periodMonth) <= m.PeriodMonthEnd
                            && revenuePercentage >= m.TargetPercentageStart
                            && revenuePercentage < m.TargetPercentageEnd
                            && m.AllocationId == allocId)
                        .Select(m => (decimal?)m.CommissionValue)
                        .FirstOrDefaultAsync();

                    // If no employee-specific rate, try generic rate
                    if (rateValue == null)
                    {
                        rateValue = await _context.DataPerformanceAgainstTargetMatrices
                            .Where(m => m.Payplan == payPlanType
                                && m.EmployeeId == "X"
                                && m.PeriodYear == Convert.ToInt32(periodYear)
                                && int.Parse(periodMonth) >= m.PeriodMonthStart
                                && int.Parse(periodMonth) <= m.PeriodMonthEnd
                                && revenuePercentage >= m.TargetPercentageStart
                                && revenuePercentage < m.TargetPercentageEnd
                                && m.AllocationId == allocId)
                            .Select(m => (decimal?)m.CommissionValue)
                            .FirstOrDefaultAsync();
                    }

                    _logger.LogDebug("First rate found: {Rate}", rateValue);

                    // Business rule: if parcel locker (270) and no rate set, use MRS rates (264)
                    if (rateValue == null && allocId == 270)
                    {
                        rateValue = await _context.DataPerformanceAgainstTargetMatrices
                            .Where(m => m.Payplan == payPlanType
                                && m.EmployeeId == employeeId
                                && m.PeriodYear == Convert.ToInt32(periodYear)
                                && int.Parse(periodMonth) >= m.PeriodMonthStart
                                && int.Parse(periodMonth) <= m.PeriodMonthEnd
                                && revenuePercentage >= m.TargetPercentageStart
                                && revenuePercentage < m.TargetPercentageEnd
                                && m.AllocationId == 264)
                            .Select(m => (decimal?)m.CommissionValue)
                            .FirstOrDefaultAsync();

                        if (rateValue == null)
                        {
                            rateValue = await _context.DataPerformanceAgainstTargetMatrices
                                .Where(m => m.Payplan == payPlanType
                                    && m.EmployeeId == "X"
                                    && m.PeriodYear == Convert.ToInt32(periodYear)
                                    && int.Parse(periodMonth) >= m.PeriodMonthStart
                                    && int.Parse(periodMonth) <= m.PeriodMonthEnd
                                    && revenuePercentage >= m.TargetPercentageStart
                                    && revenuePercentage < m.TargetPercentageEnd
                                    && m.AllocationId == 264)
                                .Select(m => (decimal?)m.CommissionValue)
                                .FirstOrDefaultAsync();
                        }
                    }
                }

                _logger.LogDebug("Rate Value to return: {Rate}", rateValue);
                return rateValue;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting revenue rate value from allocId");
                throw;
            }
        }

    }
}
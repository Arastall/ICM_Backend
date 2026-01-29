using DocumentFormat.OpenXml.Drawing.Charts;
using ICMServer.DBContext;
using ICMServer.Helpers;
using ICMServer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ICMServer.Services
{
    public interface ICreditAllocationService
    {
        Task ProcessCreditAllocationOrderAsync(string orderId);

        Task SetRevenueProcessedFlagAsync(string orderId);
    }

    public class CreditAllocationService : ICreditAllocationService
    {
        private ICMDBContext _context;
        private readonly ISystemParametersHelper _sysParams;
        private readonly IProductValidationService _productValidation;
        private readonly IRevenueCalculationService _revenueCalculation;
        private readonly IPeriodContext _periodContext;
        private readonly ILogger<CreditAllocationService> _logger;
        private readonly IServiceProvider _sp;

        public CreditAllocationService(
            ISystemParametersHelper sysParams,
            IProductValidationService productValidation,
            IRevenueCalculationService revenueCalculation,
            IPeriodContext periodContext,
            ILogger<CreditAllocationService> logger, IServiceProvider sp)
        {
            _sysParams = sysParams;
            _productValidation = productValidation;
            _revenueCalculation = revenueCalculation;
            _periodContext = periodContext;
            _logger = logger;
            _sp = sp;
        }

        public async Task ProcessCreditAllocationOrderAsync(string orderId)
        {
            try
            {
                using var scope = _sp.CreateScope();
                _context = scope.ServiceProvider.GetRequiredService<ICMDBContext>();

                _logger.LogDebug("Credit Allocation - Started");

                var orderItems = await _context.DataOrderItems
                    .Where(i => i.OrderRowId == orderId)
                    .OrderBy(i => i.OrderItemId)
                    .ToListAsync();

                _logger.LogDebug("Amount of products in order: {Count}", orderItems.Count);

                foreach (var item in orderItems)
                {
                    _logger.LogDebug("Item ID: {ItemId}", item.OrderItemId);

                    // Check if product is excluded
                    var isExcluded = await _productValidation.IsProductExcludedAsync(
                        item.ProductCode,
                        item.ProductLevel1,
                        item.ProductLevel2,
                        item.ProductLevel3);

                    if (isExcluded)
                    {
                        _logger.LogDebug("==> PRODUCT EXCLUDED !! ItemId: {ItemId}", item.OrderItemId);
                        continue;
                    }

                    // Get positions (reps) for this order
                    var positions = await _context.DataOrderPositions
                        .Where(p => p.OrderRowId == orderId)
                        .OrderBy(p => p.PositionRowId)
                        .ToListAsync();

                    _logger.LogDebug("Amount of sellers in order: {Count}", positions.Count);

                    // Process each position
                    foreach (var position in positions)
                    {
                        await _revenueCalculation.CalculateRevenueForProductAndRepAsync(
                            orderId,
                            item.OrderItemId,
                            position.PositionRowId);
                    }
                }

                await SetRevenueProcessedFlagAsync(orderId);

                _logger.LogDebug("Credit Allocation - Completed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in credit allocation for order {OrderId}", orderId);
                throw;
            }
        }

        public async Task SetRevenueProcessedFlagAsync(string orderId)
        {
            try
            {
                var processHistory = await _context.DataOrderProcessHistories
                    .FirstOrDefaultAsync(h => h.RowId == orderId);

                if (processHistory != null)
                {
                    processHistory.RevenueProcessed = true;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting revenue processed flag for order {OrderId}", orderId);
                throw;
            }
        }
    }
}
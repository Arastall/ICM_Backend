using ICMServer.DBContext;
using ICMServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ICMServer.Services
{
    public interface IProductValidationService
    {
        Task<bool> IsProductExcludedAsync(string productCode, string level1, string level2, string level3);
        Task<string> GetProductAllocationIdsAsync(string orderId, string itemId, bool isEngineer);
    }

    public class ProductValidationService : IProductValidationService
    {
        private ICMDBContext _context;
        private readonly ILogger<ProductValidationService> _logger;
        private readonly IServiceProvider _sp;

        public ProductValidationService(ILogger<ProductValidationService> logger, IServiceProvider sp)
        {
            _logger = logger;
            _sp = sp;
        }

        public async Task<bool> IsProductExcludedAsync(string productCode, string level1, string level2, string level3)
        {
            try
            {
                using var scope = _sp.CreateScope();
                _context = scope.ServiceProvider.GetRequiredService<ICMDBContext>();

                int count = 0;

                // Hardcoded exclusions
                if (level1 == "00010")
                {
                    if (level2 == "00104" && (level3 == "IMPR" || level3 == "TOUC"))
                    {
                        count = 1;
                    }
                    else if (level2 == "00105" && level3 == "IMPR")
                    {
                        count = 1;
                    }
                }

                if (count == 0)
                {
                    // Check level-based exclusion
                    var levelBased = await _context.DataProductsIncludeds
                        .Where(p => p.ProductLevel1 == level1
                            && p.ProductLevel2 == level2
                            && p.ProductLevel3 == level3
                            && p.Included == 0)
                        .Select(p => p.LevelBased)
                        .FirstOrDefaultAsync();

                    if (levelBased == 1)
                    {
                        count = 1;
                    }
                    else if (levelBased != null)
                    {
                        // Check product code-based exclusion
                        count = await _context.DataProductsIncludeds
                            .Where(p => p.ProductLevel1 == level1
                                && p.ProductLevel2 == level2
                                && p.ProductLevel3 == level3
                                && p.ProductCode == productCode
                                && p.Included == 0)
                            .CountAsync();
                    }
                }

                return count > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if product is excluded");
                throw;
            }
        }

        public async Task<string> GetProductAllocationIdsAsync(string orderId, string itemId, bool isEngineer)
        {
            try
            {
                using var scope = _sp.CreateScope();
                _context = scope.ServiceProvider.GetRequiredService<ICMDBContext>();

                var orderItem = await _context.DataOrderItems
                    .Where(i => i.OrderItemId == itemId && i.OrderRowId == orderId)
                    .Select(i => new
                    {
                        i.ProductCode,
                        i.ProductLevel1,
                        i.ProductLevel2,
                        i.ProductLevel3,
                        i.ProductDesc
                    })
                    .FirstOrDefaultAsync();

                if (orderItem == null)
                {
                    _logger.LogWarning("Order item {ItemId} not found in ORDER ITEMS", itemId);
                    return null;
                }

                var productCode = orderItem.ProductCode ?? "";
                var level1 = orderItem.ProductLevel1 ?? "";
                var level2 = orderItem.ProductLevel2 ?? "";
                var level3 = orderItem.ProductLevel3 ?? "";

                // Vérification des champs requis AVANT la logique
                if (string.IsNullOrEmpty(productCode) || string.IsNullOrEmpty(level1)
                    || string.IsNullOrEmpty(level2) || string.IsNullOrEmpty(level3))
                {
                    _logger.LogWarning("Product {ItemId} not properly defined in ORDER ITEMS", itemId);
                    await AddToUnknownProductsAsync(orderItem.ProductDesc, productCode, level1, level2, level3);
                    return null;
                }

                List<int> targetedKpis;

                if (isEngineer)
                {
                    targetedKpis = new List<int> { 277 };
                }
                else
                {
                    targetedKpis = await _context.DataProductsIncludeds
                        .Where(p => p.ProductLevel1 == level1
                            && p.ProductLevel2 == level2
                            && p.ProductLevel3 == level3
                            && p.Included == 1
                            && p.AllocationId != 277)
                        .Select(p => p.AllocationId.Value)
                        .Distinct()
                        .ToListAsync();
                }

                // Étape 1: Vérifier si une config existe pour ces levels
                var productConfig = await _context.DataProductsIncludeds
                    .Where(p => p.ProductLevel1 == level1
                        && p.ProductLevel2 == level2
                        && p.ProductLevel3 == level3
                        && p.Included == 1
                        && targetedKpis.Contains(p.AllocationId.Value))
                    .Select(p => p.LevelBased)
                    .FirstOrDefaultAsync();

                if (productConfig == null)
                {
                    _logger.LogWarning("Product {ItemId} not authorized or set", itemId);
                    await AddToUnknownProductsAsync(orderItem.ProductDesc, productCode, level1, level2, level3);
                    return null;
                }

                List<int?> allocations;

                // Étape 2: Si LevelBased == 1, on cherche par levels uniquement
                if (productConfig == 1)
                {
                    allocations = await _context.DataProductsIncludeds
                        .Where(p => p.ProductLevel1 == level1
                            && p.ProductLevel2 == level2
                            && p.ProductLevel3 == level3
                            && p.LevelBased == 1
                            && p.Included == 1
                            && targetedKpis.Contains(p.AllocationId.Value))
                        .Select(p => p.AllocationId)
                        .Distinct()
                        .ToListAsync();
                }
                // Étape 3: Sinon (LevelBased == 0), on cherche par product code
                else
                {
                    allocations = await _context.DataProductsIncludeds
                        .Where(p => p.ProductLevel1 == level1
                            && p.ProductLevel2 == level2
                            && p.ProductLevel3 == level3
                            && p.ProductCode == productCode
                            && p.LevelBased == 0
                            && p.Included == 1
                            && targetedKpis.Contains(p.AllocationId.Value))
                        .Select(p => p.AllocationId)
                        .ToListAsync();

                    // Si aucune allocation trouvée par code, fallback sur level-based
                    if (!allocations.Any())
                    {
                        allocations = await _context.DataProductsIncludeds
                            .Where(p => p.ProductLevel1 == level1
                                && p.ProductLevel2 == level2
                                && p.ProductLevel3 == level3
                                && p.LevelBased == 1
                                && p.Included == 1
                                && targetedKpis.Contains(p.AllocationId.Value))
                            .Select(p => p.AllocationId)
                            .ToListAsync();
                    }
                }

                if (!allocations.Any())
                {
                    _logger.LogWarning("Product {ItemId} not authorized or set", itemId);
                    await AddToUnknownProductsAsync(orderItem.ProductDesc, productCode, level1, level2, level3);
                    return null;
                }

                var kpis = string.Join(",", allocations);
                _logger.LogInformation("Product {ItemId} authorized with KPIs: {Kpis}", itemId, kpis);
                return kpis;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if product is set");
                throw;
            }
        }

        private async Task AddToUnknownProductsAsync(string productDesc, string productCode, string level1, string level2, string level3)
        {
            using var scope = _sp.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<ICMDBContext>();

            var exists = await _context.DataProductsUnknowns
                .AnyAsync(p => p.ProductLevel1 == level1
                    && p.ProductLevel2 == level2
                    && p.ProductLevel3 == level3
                    && p.ProductCode == productCode);

            if (!exists)
            {
                _context.DataProductsUnknowns.Add(new DataProductsUnknown
                {
                    ProductDesc = productDesc,
                    ProductCode = productCode,
                    ProductLevel1 = level1,
                    ProductLevel2 = level2,
                    ProductLevel3 = level3
                });

                await _context.SaveChangesAsync();
            }
        }
    }
}

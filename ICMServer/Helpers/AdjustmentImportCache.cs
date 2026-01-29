using ICMServer.Models;
using Microsoft.Extensions.Caching.Memory;

namespace ICMServer.Helpers
{
    // Service pour stocker temporairement les adjustments parsés
    public class AdjustmentImportCache
    {
        private readonly IMemoryCache _cache;

        public AdjustmentImportCache(IMemoryCache cache)
        {
            _cache = cache;
        }

        public string Store(List<DataManualAdjustment> adjustments)
        {
            var key = Guid.NewGuid().ToString();
            _cache.Set(key, adjustments, TimeSpan.FromMinutes(10)); // Expire après 10 min
            return key;
        }

        public List<DataManualAdjustment>? Get(string key)
        {
            _cache.TryGetValue(key, out List<DataManualAdjustment>? adjustments);
            return adjustments;
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}

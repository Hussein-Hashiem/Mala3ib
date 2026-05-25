using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Mala3ib.BLL.Service.Implementation
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;

        public CacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
        {
            var data = await _distributedCache.GetStringAsync(key, cancellationToken);

            return string.IsNullOrEmpty(data) ? null : JsonSerializer.Deserialize<T>(data);
        }

        public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
        {
            await _distributedCache.SetStringAsync(key, JsonSerializer.Serialize(value), cancellationToken);
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            await _distributedCache.RemoveAsync(key, cancellationToken);
        }
    }
}

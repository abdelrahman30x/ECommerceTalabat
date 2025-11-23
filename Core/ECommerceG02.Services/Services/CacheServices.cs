using ECommerceG02.Abstractions.Services;
using ECommerceG02.Domian.Contacts.Repos;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ECommerceG02.Services.Services
{
    public class CacheServices(ICacheRepository cacheRepository, ILogger<CacheServices> logger) : ICacheServices
    {
        private readonly ICacheRepository _cache = cacheRepository;
        private readonly ILogger<CacheServices> _logger = logger;

        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                var cached = await _cache.GetAsync<T>(key);
                if (cached is not null)
                    _logger.LogInformation("Cache hit: {Key}", key);
                return cached;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cache GET failed for key: {Key}", key);
                return default;
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan expiry)
        {
            try
            {
                await _cache.SetAsync(key, value, expiry);
                _logger.LogInformation("Cache set: {Key}", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cache SET failed for key: {Key}", key);
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                await _cache.DeleteAsync(key);
                _logger.LogInformation("Cache removed: {Key}", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cache DELETE failed for key: {Key}", key);
            }
        }
    }
}

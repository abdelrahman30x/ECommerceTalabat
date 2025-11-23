using ECommerceG02.Domian.Contacts.Repos;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerceG02.Presistence.Repos
{
    public class CacheRepository(IConnectionMultiplexer connection, ILogger<CacheRepository> logger)
    : ICacheRepository
    {
        private readonly IDatabase _db = connection.GetDatabase();
        private readonly ILogger<CacheRepository> _logger = logger;

        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                var data = await _db.StringGetAsync(key);
                if (data.IsNullOrEmpty) return default;

                return JsonSerializer.Deserialize<T>(data!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis GET failed for key: {Key}", key);
                return default;
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            try
            {
                var json = JsonSerializer.Serialize(value);
                await _db.StringSetAsync(key, json, expiry);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis SET failed for key: {Key}", key);
            }
        }

        public async Task<bool> DeleteAsync(string key)
        {
            try
            {
                return await _db.KeyDeleteAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis DELETE failed for key: {Key}", key);
                return false;
            }
        }
    }

}

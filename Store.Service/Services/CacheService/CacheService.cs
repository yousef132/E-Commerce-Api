using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Service.Services.CacheService
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase database;
        public CacheService(IConnectionMultiplexer redis)
        {
            database  = redis.GetDatabase();
        }
        public async Task<string> GetCacheResponseAsync(string key)
        {
                string cachedResponse = await  database.StringGetAsync(key);
            if (string.IsNullOrEmpty(cachedResponse))
                return null;

            return cachedResponse.ToString();
        }

        public async Task SetCacheResponseAsync(string key, object response, TimeSpan timeToLive)
        {
            if (response == null)
                return;

            var serializedResponse = JsonSerializer.Serialize(response);

            await database.StringSetAsync(key, serializedResponse, timeToLive);
        }
    }
}

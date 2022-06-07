using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Redis
{
    public class Cache        
    {
        private readonly IDatabase _cache;
        private readonly IConnectionMultiplexer _multiplexer;

        public Cache(IConnectionMultiplexer multiplexer)
        {
            _multiplexer = multiplexer;
            _cache = multiplexer.GetDatabase();
        }

        public async Task<T> Get<T>(string key) where T : class
        {
            var cachedResponse = await _cache.StringGetAsync((RedisKey)key);
            return JsonConvert.DeserializeObject<T>(cachedResponse);
        }

        public async Task<List<T>> GetMany<T>(string listOfKeys) where T : class
        {
            string[] strings = listOfKeys.Split(",");
            RedisKey[] keys = strings.Select(key => (RedisKey)key).ToArray();
            var response = await _cache.StringGetAsync(keys);
            return response.Select(d => JsonConvert.DeserializeObject<T>(d)).ToList();
        }

        public async Task Set<T>(string key, T value, DistributedCacheEntryOptions options) where T: class
        {
            var response = JsonConvert.SerializeObject(value, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            await _cache.StringSetAsync(key, Encoding.UTF8.GetBytes(response));
        }
    }
}

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SFA.DAS.FindApprenticeshipTraining.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Infrastructure.Services
{
    public class CacheStorageService : ICacheStorageService
    {
        private readonly IDistributedCache _distributedCache;
        
        public CacheStorageService (IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        
        public async Task<T> RetrieveFromCache<T>(string key)
        {
            var json = await _distributedCache.GetStringAsync(key);
            return json == null ? default : JsonConvert.DeserializeObject<T>(json);
        }

        public async Task SaveToCache<T>(string key, T item, int expirationInHours)
        {
            var json = JsonConvert.SerializeObject(item);

            await _distributedCache.SetStringAsync(key, json, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(expirationInHours)
            });
        }

        public async Task DeleteFromCache(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }
    }

    
}
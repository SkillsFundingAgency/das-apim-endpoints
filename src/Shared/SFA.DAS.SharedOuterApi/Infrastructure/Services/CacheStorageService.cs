using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Infrastructure.Services
{
    public class CacheStorageService : ICacheStorageService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IConfiguration _configuration;
        
        public CacheStorageService (IDistributedCache distributedCache, IConfiguration configuration)
        {
            _distributedCache = distributedCache;
            _configuration = configuration;
        }
        
        public async Task<T> RetrieveFromCache<T>(string key)
        {
            var json = await _distributedCache.GetStringAsync($"{_configuration["ConfigNames"].Split(",")[0]}_{key}");
            return json == null ? default : JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions{});
        }

        public async Task SaveToCache<T>(string key, T item, int expirationInHours)
        {
            await SaveToCache(key, item, TimeSpan.FromHours(expirationInHours));
        }

        public async Task SaveToCache<T>(string key, T item, TimeSpan expiryTimeFromNow)
        {
            var json = JsonSerializer.Serialize(item);

            await _distributedCache.SetStringAsync($"{_configuration["ConfigNames"].Split(",")[0]}_{key}", json, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiryTimeFromNow
            });
        }

        public async Task DeleteFromCache(string key)
        {
            await _distributedCache.RemoveAsync($"{_configuration["ConfigNames"]}_{key}");
        }

        public async Task AddToCacheKeyRegistry(string registryName, string key)
        {
            var cacheKey = $"{_configuration["ConfigNames"].Split(",")[0]}_{registryName}";
            var existingJson = await _distributedCache.GetStringAsync(cacheKey);

            var keys = string.IsNullOrEmpty(existingJson)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(existingJson, new JsonSerializerOptions());

            if (!keys.Contains(key))
            {
                keys.Add(key);
                var updatedJson = JsonSerializer.Serialize(keys);
                await _distributedCache.SetStringAsync(cacheKey, updatedJson, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
                });
            }
        }

        public async Task<List<string>> GetCacheKeyRegistry(string registryName)
        {
            var cacheKey = $"{_configuration["ConfigNames"].Split(",")[0]}_{registryName}";
            var json = await _distributedCache.GetStringAsync(cacheKey);

            return string.IsNullOrEmpty(json)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(json, new JsonSerializerOptions());
        }
    }
}

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Infrastructure.Services
{
    public class CacheStorageService : ICacheStorageService
    {
        private readonly IDistributedCache _distributedCache;
        private const int ExpirationInHours = 1;

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

        public async Task UpdateCachedItems(Task<GetSectorsListResponse> sectorsTask,
            Task<GetLevelsListResponse> levelsTask,
            Task<GetStandardsListResponse> standardsTask,
            SaveToCache saveToCache)
        {
            if (saveToCache.Sectors)
            {
                await SaveToCache(nameof(GetSectorsListResponse), sectorsTask.Result, ExpirationInHours);
            }

            if (saveToCache.Levels)
            {
                await SaveToCache(nameof(GetLevelsListResponse), levelsTask.Result, ExpirationInHours);
            }

            if (saveToCache.Standards)
            {
                await SaveToCache(nameof(GetStandardsListResponse), standardsTask.Result, ExpirationInHours);
            }
        }

        public Task<TResponse> GetRequest<TResponse>(ICoursesApiClient<CoursesApiConfiguration> client, IGetApiRequest request, string keyName, out bool updateCache)
        {
            Task<TResponse> itemsTask;
            updateCache = false;

            var itemFromCache = RetrieveFromCache<TResponse>(keyName).Result;

            if (itemFromCache != null)
            {
                itemsTask = Task.FromResult(itemFromCache);
            }
            else
            {
                itemsTask = client.Get<TResponse>(request);
                updateCache = true;
            }

            return itemsTask;
        }

        public bool FilterApplied(GetStandardsListRequest request)
        {
            if (String.IsNullOrEmpty(request.Keyword) && request.Levels == null && request.RouteIds == null)
                return false;
            return true;
        }
    }

    public class SaveToCache
    {
        public bool Sectors { get; set; }
        public bool Levels { get; set; }
        public bool Standards { get; set; }
    }
}
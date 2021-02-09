using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses
{
    internal class CacheHelper
    {
        private const int ExpirationInHours = 1;
        private readonly ICacheStorageService _cacheStorageService;

        public CacheHelper (ICacheStorageService cacheStorageService)
        {
            _cacheStorageService = cacheStorageService;
        }
        
        public async Task UpdateCachedItems(Task<GetSectorsListResponse> sectorsTask,
            Task<GetLevelsListResponse> levelsTask,
            Task<GetStandardsListResponse> standardsTask,
            SaveToCache saveToCache)
        {
            if (saveToCache.Sectors)
            {
                await _cacheStorageService.SaveToCache(nameof(GetSectorsListResponse), sectorsTask.Result, ExpirationInHours);
            }

            if (saveToCache.Levels)
            {
                await _cacheStorageService.SaveToCache(nameof(GetLevelsListResponse), levelsTask.Result, ExpirationInHours);
            }

            if (saveToCache.Standards)
            {
                await _cacheStorageService.SaveToCache(nameof(GetStandardsListResponse), standardsTask.Result, ExpirationInHours);
            }
        }

        public Task<TResponse> GetRequest<TResponse>(ICoursesApiClient<CoursesApiConfiguration> client, IGetApiRequest request, string keyName, out bool updateCache)
        {
            Task<TResponse> itemsTask;
            updateCache = false;

            var itemFromCache = _cacheStorageService.RetrieveFromCache<TResponse>(keyName).Result;

            if (itemFromCache != null)
            {
                itemsTask = Task.FromResult(itemFromCache);

                if (itemFromCache.GetType() == typeof(GetStandardsListResponse))
                {
                    var castItem = itemFromCache as GetStandardsListResponse;
                    if (castItem.Total < 1)
                    {
                        itemsTask = client.Get<TResponse>(request);
                        updateCache = true;
                    }
                }
            }
            else
            {
                itemsTask = client.Get<TResponse>(request);
                updateCache = true;
            }

            return itemsTask;
        }

        public bool FilterApplied(GetAvailableToStartStandardsListRequest request)
        {
            return !string.IsNullOrEmpty(request.Keyword) || request.Levels.Any() || request.RouteIds.Any() || request.OrderBy != CoursesOrderBy.Score;
        }

        internal class SaveToCache
        {
            public bool Sectors { get; set; }
            public bool Levels { get; set; }
            public bool Standards { get; set; }
        }
    }
}
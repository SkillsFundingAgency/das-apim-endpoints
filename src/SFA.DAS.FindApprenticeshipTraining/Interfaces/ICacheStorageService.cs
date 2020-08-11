using System.Threading.Tasks;
using SFA.DAS.FindApprenticeshipTraining.Infrastructure.Services;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Interfaces
{
    public interface ICacheStorageService
    {
        Task<T> RetrieveFromCache<T>(string key);
        Task SaveToCache<T>(string key, T item, int expirationInHours);
        Task DeleteFromCache(string key);
        Task UpdateCachedItems(Task<GetSectorsListResponse> sectorsTask, Task<GetLevelsListResponse> levelsTask,
            Task<GetStandardsListResponse> standardsTask, SaveToCache saveToCache);

        Task<TResponse> GetRequest<TResponse>(ICoursesApiClient<CoursesApiConfiguration> client,
            IGetApiRequest request, string keyName, out bool updateCache);

        public bool FilterApplied(GetStandardsListRequest request);
    }
}
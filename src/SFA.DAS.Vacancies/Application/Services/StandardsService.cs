using System.Numerics;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Vacancies.InnerApi.Responses;
using SFA.DAS.Vacancies.Interfaces;

namespace SFA.DAS.Vacancies.Application.Services
{
    public class StandardsService  : IStandardsService
    {
        private const int CourseCacheExpiryInHours = 4;
        private readonly ICacheStorageService _cacheStorageService;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public StandardsService (ICacheStorageService cacheStorageService, ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _cacheStorageService = cacheStorageService;
            _coursesApiClient = coursesApiClient;
        }
        public async Task<GetStandardsListResponse> GetStandards()
        {
            var cachedCourses =
                await _cacheStorageService.RetrieveFromCache<GetStandardsListResponse>(
                    nameof(GetStandardsListResponse));

            if (cachedCourses != null)
            {
                return cachedCourses;
            }

            var apiCourses = await _coursesApiClient.Get<GetStandardsListResponse>(new GetActiveStandardsListRequest());

            await _cacheStorageService.SaveToCache(nameof(GetStandardsListResponse), apiCourses, CourseCacheExpiryInHours);

            return apiCourses;
        }
    }
}
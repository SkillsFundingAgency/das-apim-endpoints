using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Services
{
    public class CourseService : ICourseService
    {
        private const int CourseCacheExpiryInHours = 4;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly ICacheStorageService _cacheStorageService;

        public CourseService (ICoursesApiClient<CoursesApiConfiguration> coursesApiClient, ICacheStorageService cacheStorageService)
        {
            _coursesApiClient = coursesApiClient;
            _cacheStorageService = cacheStorageService;
        }
        
        public async Task<GetRoutesListResponse> GetRoutes()
        {
            var response = await _cacheStorageService.RetrieveFromCache<GetRoutesListResponse>(nameof(GetRoutesListResponse));
            if (response == null)
            {
                response = await _coursesApiClient.Get<GetRoutesListResponse>(new GetRoutesListRequest());

                await _cacheStorageService.SaveToCache(nameof(GetRoutesListResponse), response, 23);
            }

            return response;
        }

        public async Task<T> GetActiveStandards<T>(string cacheItemName)
        {
            var cachedCourses =
                await _cacheStorageService.RetrieveFromCache<T>(
                    cacheItemName);

            if (cachedCourses != null)
            {
                return cachedCourses;
            }

            var apiCourses = await _coursesApiClient.Get<T>(new GetActiveStandardsListRequest());

            await _cacheStorageService.SaveToCache(cacheItemName, apiCourses, CourseCacheExpiryInHours);

            return apiCourses;
        }
    }
}
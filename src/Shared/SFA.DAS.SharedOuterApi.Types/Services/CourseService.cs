
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;

namespace SFA.DAS.SharedOuterApi.Types.Services
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

        public async Task<GetCourseLevelsListResponse> GetLevels()
        {
            var response = await _cacheStorageService.RetrieveFromCache<GetCourseLevelsListResponse>(nameof(GetCourseLevelsListResponse));
            if (response == null)
            {
                response = await _coursesApiClient.Get<GetCourseLevelsListResponse>(new GetCourseLevelsListRequest());

                await _cacheStorageService.SaveToCache(nameof(GetCourseLevelsListResponse), response, 23);
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
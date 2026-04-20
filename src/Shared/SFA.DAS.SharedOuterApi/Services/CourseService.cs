using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Services
{
    public class CourseService(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
        ICacheStorageService cacheStorageService)
        : ICourseService
    {
        private const int CourseCacheExpiryInHours = 4;

        public async Task<GetRoutesListResponse> GetRoutes()
        {
            var response = await cacheStorageService.RetrieveFromCache<GetRoutesListResponse>(nameof(GetRoutesListResponse));
            if (response == null)
            {
                response = await coursesApiClient.Get<GetRoutesListResponse>(new GetRoutesListRequest());

                await cacheStorageService.SaveToCache(nameof(GetRoutesListResponse), response, 23);
            }

            return response;
        }

        public async Task<GetCourseLevelsListResponse> GetLevels()
        {
            var response = await cacheStorageService.RetrieveFromCache<GetCourseLevelsListResponse>(nameof(GetCourseLevelsListResponse));
            if (response == null)
            {
                response = await coursesApiClient.Get<GetCourseLevelsListResponse>(new GetCourseLevelsListRequest());

                await cacheStorageService.SaveToCache(nameof(GetCourseLevelsListResponse), response, 23);
            }
            return response;
        }

        public async Task<T> GetActiveStandards<T>(string cacheItemName)
        {
            var cachedCourses =
                await cacheStorageService.RetrieveFromCache<T>(
                    cacheItemName);

            if (cachedCourses != null)
            {
                return cachedCourses;
            }

            var apiCourses = await coursesApiClient.Get<T>(new GetActiveStandardsListRequest());

            await cacheStorageService.SaveToCache(cacheItemName, apiCourses, CourseCacheExpiryInHours);

            return apiCourses;
        }
    }
}
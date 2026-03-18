using System.Threading.Tasks;
using SFA.DAS.FindEpao.InnerApi.Responses;
using SFA.DAS.FindEpao.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindEpao.Application.Courses.Services
{
    public class CachedCoursesService : ICachedCoursesService
    {
        private const int DeliveryAreaCacheDurationInHours = 1;

        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly ICacheStorageService _cacheStorageService;

        public CachedCoursesService(
            ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
            ICacheStorageService cacheStorageService)
        {
            _coursesApiClient = coursesApiClient;
            _cacheStorageService = cacheStorageService;
        }

        public async Task<GetStandardsListResponse> GetCourses()
        {
            var courses = await _cacheStorageService
                .RetrieveFromCache<GetStandardsListResponse>(nameof(GetStandardsListResponse));
            
            if (courses != null) 
                return courses;
            
            courses = await _coursesApiClient.Get<GetStandardsListResponse>(new GetActiveStandardsListRequest());
            await _cacheStorageService.SaveToCache(nameof(GetStandardsListResponse), courses, DeliveryAreaCacheDurationInHours);

            return courses;
        }
    }
}

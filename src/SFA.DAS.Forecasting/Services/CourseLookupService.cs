using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Forecasting.InnerApi.Requests;
using SFA.DAS.Forecasting.InnerApi.Responses;
using SFA.DAS.Forecasting.Models.Courses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Forecasting.Services
{
    public interface ICourseLookupService
    {
        public Task<List<Course>> GetAllCourses();
    }

    public class CourseLookupService : ICourseLookupService
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly ICacheStorageService _cacheStorageService;
        private const string CacheKey = "CourseLookupService.GetAllCourses";
        private const int CacheExpiryInHours = 2;

        public CourseLookupService(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient, ICacheStorageService cacheStorageService)
        {
            _coursesApiClient = coursesApiClient;
            _cacheStorageService = cacheStorageService;
        }

        public async Task<List<Course>> GetAllCourses()
        {
            var cacheResult = await _cacheStorageService.RetrieveFromCache<List<Course>>($"{CacheKey}");
            if (cacheResult != null) return cacheResult;

            var result = await GetAllCoursesFromApi();

            await _cacheStorageService.SaveToCache($"{CacheKey}", result, CacheExpiryInHours);
            return result;
        }

        private async Task<List<Course>> GetAllCoursesFromApi()
        {
            var standardsResponseTask = _coursesApiClient.Get<GetStandardsListResponse>(new GetStandardsLookupRequest());
            var frameworksResponseTask = _coursesApiClient.Get<GetFrameworksListResponse>(new GetFrameworksRequest());

            await Task.WhenAll(standardsResponseTask, frameworksResponseTask);

            var standardCourses = standardsResponseTask.Result.Standards.Select(x => new Course
                { Id = x.LarsCode.ToString(), Level = x.Level, Title = x.Title }).ToList();

            var frameworkCourses = frameworksResponseTask.Result.Frameworks.Select(x => new Course
                { Id = x.Id, Title = x.Title, Level = x.Level }).ToList();

            return standardCourses.Union(frameworkCourses).ToList();
        }
    }
}

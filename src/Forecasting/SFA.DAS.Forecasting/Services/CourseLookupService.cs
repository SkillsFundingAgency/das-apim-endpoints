using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Forecasting.InnerApi.Requests;
using SFA.DAS.Forecasting.InnerApi.Responses;
using SFA.DAS.Forecasting.Models.Courses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Forecasting.Services;

public interface ICourseLookupService
{
    public Task<List<Course>> GetAllCourses();
}

public class CourseLookupService(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient, ICacheStorageService cacheStorageService)
    : ICourseLookupService
{
    private const string CacheKey = "CourseLookupService.GetAllCourses";
    private const int CacheExpiryInHours = 2;

    public async Task<List<Course>> GetAllCourses()
    {
        var cacheResult = await cacheStorageService.RetrieveFromCache<List<Course>>($"{CacheKey}");
        if (cacheResult != null) return cacheResult;

        var result = await GetAllCoursesFromApi();

        await cacheStorageService.SaveToCache($"{CacheKey}", result, CacheExpiryInHours);
        
        return result;
    }

    private async Task<List<Course>> GetAllCoursesFromApi()
    {
        var standardsResponseTask = coursesApiClient.Get<GetStandardsListResponse>(new GetAllStandardsRequest());
        var frameworksResponseTask = coursesApiClient.Get<GetFrameworksListResponse>(new GetFrameworksRequest());

        await Task.WhenAll(standardsResponseTask, frameworksResponseTask);

        var standardCourses = standardsResponseTask
            .Result
            .Standards
            .Select(x => new Course
            {
                Id = x.LarsCode.ToString(),
                Level = x.Level,
                Title = x.Title
            }).ToList();

        var frameworkCourses = frameworksResponseTask
            .Result
            .Frameworks
            .Select(x => new Course
            {
                Id = x.Id,
                Title = x.Title,
                Level = x.Level
            }).ToList();

        return standardCourses.Union(frameworkCourses).ToList();
    }
}

using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Services;

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

    public async Task<CourseLookupDetailResponse> GetCourseLookupDetailsById(string courseCode)
    {
        string cacheItemName = nameof(CourseLookupDetailResponse) + "_" + courseCode;
        var response = await cacheStorageService.RetrieveFromCache<CourseLookupDetailResponse>(cacheItemName);

        if (response == null)
        {
            response = await coursesApiClient.Get<CourseLookupDetailResponse>(new GetCourseLookupDetailsByIdRequest(courseCode));
            await cacheStorageService.SaveToCache(cacheItemName, response, CourseCacheExpiryInHours);
        }

        return response;
    }

    public async Task<StandardDetailResponse> GetStandardDetailsById(string standardId)
    {
        string cacheItemName = nameof(StandardDetailResponse) + "_" + standardId;
        var response = await cacheStorageService.RetrieveFromCache<StandardDetailResponse>(cacheItemName);

        if (response == null)
        {
            response = await coursesApiClient.Get<StandardDetailResponse>(new GetStandardDetailsByIdRequest(standardId));
            await cacheStorageService.SaveToCache(cacheItemName, response, CourseCacheExpiryInHours);
        }

        return response;
    }
}
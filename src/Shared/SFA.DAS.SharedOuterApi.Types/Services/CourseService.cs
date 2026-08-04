using SFA.DAS.Apim.Shared.Extensions;
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

    public async Task<CourseLookupResult> GetCourseLookupDetailsById(string courseCode)
    {
        var cacheItemName = nameof(CourseLookupDetailResponse) + "_" + courseCode;
        var cached = await cacheStorageService.RetrieveFromCache<CourseLookupDetailResponse>(cacheItemName);

        if (cached != null)
        {
            return new CourseLookupResult(CourseLookupStatus.Found, cached);
        }

        var response = await coursesApiClient.GetWithResponseCode<CourseLookupDetailResponse>(new GetCourseLookupDetailsByIdRequest(courseCode));

        if (response.StatusCode.IsSuccessStatusCode())
        {
            // Only cache successful lookups - caching a not-found or transient-failure response
            // would keep returning that failure for the cache lifetime even after it clears.
            if (response.Body != null)
            {
                await cacheStorageService.SaveToCache(cacheItemName, response.Body, CourseCacheExpiryInHours);
            }

            return new CourseLookupResult(CourseLookupStatus.Found, response.Body);
        }

        return (int)response.StatusCode >= 500
            ? new CourseLookupResult(CourseLookupStatus.Unavailable, null)
            : new CourseLookupResult(CourseLookupStatus.NotFound, null);
    }

    public async Task<StandardDetailResponse> GetStandardDetailsById(string standardId)
    {
        var cacheItemName = nameof(StandardDetailResponse) + "_" + standardId;
        var response = await cacheStorageService.RetrieveFromCache<StandardDetailResponse>(cacheItemName);

        if (response == null)
        {
            response = await coursesApiClient.Get<StandardDetailResponse>(new GetStandardDetailsByIdRequest(standardId));
            await cacheStorageService.SaveToCache(cacheItemName, response, CourseCacheExpiryInHours);
        }

        return response;
    }
}
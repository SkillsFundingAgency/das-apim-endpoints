using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Services;

public class CachedStandardDetailsService : ICachedStandardDetailsService
{
    private const int StandardDetailsCacheDurationInHours = 4;

    private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
    private readonly ICacheStorageService _cacheStorageService;

    public CachedStandardDetailsService(
        ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
        ICacheStorageService cacheStorageService)
    {
        _coursesApiClient = coursesApiClient;
        _cacheStorageService = cacheStorageService;
    }

    public async Task<GetKsbsForCourseOptionResponse> GetKsbsForCourseOption(string larsCode)
    {
        var ksbsCacheKey = $"{nameof(GetKsbsForCourseOptionResponse)}-{larsCode}";
        var cachedKsbsForCourseOption = await _cacheStorageService.RetrieveFromCache<GetKsbsForCourseOptionResponse>(ksbsCacheKey);

        if (cachedKsbsForCourseOption != null)
            return cachedKsbsForCourseOption;

        var apiResponse = await _coursesApiClient.GetWithResponseCode<GetKsbsForCourseOptionResponse>(new GetKsbsForCourseOptionRequest(larsCode));
        var ksbsResponse = apiResponse.Body;
        await _cacheStorageService.SaveToCache(ksbsCacheKey, ksbsResponse, StandardDetailsCacheDurationInHours);
        return ksbsResponse;
    }

    public async Task<StandardDetailsLookupResponse> GetStandardDetails(string larsCode)
    {
        var standardDetailsCacheKey = $"{nameof(StandardDetailsLookupResponse)}-{larsCode}";
        var cachedStandardDetails = await _cacheStorageService
            .RetrieveFromCache<StandardDetailsLookupResponse>(standardDetailsCacheKey);

        if (cachedStandardDetails != null)
            return cachedStandardDetails;

        var apiResponse = await _coursesApiClient.GetWithResponseCode<StandardDetailsLookupResponse>(new GetStandardDetailsLookupRequest(larsCode));
        cachedStandardDetails = apiResponse.Body;
        await _cacheStorageService.SaveToCache(standardDetailsCacheKey, cachedStandardDetails, StandardDetailsCacheDurationInHours);

        return cachedStandardDetails;
    }
}

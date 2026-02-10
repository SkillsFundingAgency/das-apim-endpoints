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

    public async Task<StandardDetailResponse> GetStandardDetails(string larsCode)
    {
        var standardDetailsCacheKey = $"{nameof(StandardDetailResponse)}-{larsCode}";
        var cachedStandardDetails = await _cacheStorageService
            .RetrieveFromCache<StandardDetailResponse>(standardDetailsCacheKey);

        if (cachedStandardDetails != null)
            return cachedStandardDetails;

        var apiResponse = await _coursesApiClient.GetWithResponseCode<StandardDetailResponse>(new GetStandardDetailsByIdRequest(larsCode));
        cachedStandardDetails = apiResponse.Body;
        await _cacheStorageService.SaveToCache(standardDetailsCacheKey, cachedStandardDetails, StandardDetailsCacheDurationInHours);

        return cachedStandardDetails;
    }
}

using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

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

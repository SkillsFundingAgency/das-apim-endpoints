using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Rofjaa;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Rofjaa;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.LearnerData.Services;

public interface IFjaaAgenciesService
{
    Task<GetAgenciesResponse> GetAgencies(CancellationToken cancellationToken = default);
}

public class FjaaAgenciesService(
    IFjaaApiClient<FjaaApiConfiguration> fjaaApiClient,
    ICacheStorageService cacheStorageService) : IFjaaAgenciesService
{
    private const string CacheKey = "Fjaa.GetAgenciesResponse";
    private const int CacheExpiryInHours = 2;

    public async Task<GetAgenciesResponse> GetAgencies(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var cached = await cacheStorageService.RetrieveFromCache<GetAgenciesResponse>(CacheKey);
        if (cached is not null)
        {
            return cached;
        }

        var response = await fjaaApiClient.Get<GetAgenciesResponse>(new GetAgenciesQuery());
        await cacheStorageService.SaveToCache(CacheKey, response, CacheExpiryInHours);
        return response;
    }
}

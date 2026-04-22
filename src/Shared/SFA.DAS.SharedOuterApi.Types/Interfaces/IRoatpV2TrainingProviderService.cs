using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.RoatpV2;

namespace SFA.DAS.SharedOuterApi.Types.Interfaces;

public interface IRoatpV2TrainingProviderService
{
    Task<GetProviderSummaryResponse> GetProviderSummary(int ukprn);
    Task<GetProvidersResponse> GetProviders(CancellationToken cancellationToken);
    Task<GetProvidersResponse> GetProviders(bool live);
}
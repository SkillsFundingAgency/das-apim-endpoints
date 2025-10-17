using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Interfaces;

public interface IRoatpV2TrainingProviderService
{
    Task<GetProviderSummaryResponse> GetProviderSummary(int ukprn);
    Task<GetProvidersResponse> GetProviders(CancellationToken cancellationToken);
    Task<GetProvidersResponse> GetProviders(bool live);
}
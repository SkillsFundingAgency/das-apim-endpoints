using RestEase;
using SFA.DAS.RoatpProviderModeration.Application.InnerApi.Requests;
using SFA.DAS.RoatpProviderModeration.Application.InnerApi.Responses;

namespace SFA.DAS.RoatpProviderModeration.Application.Infrastructure;
public interface IRoatpV2ApiClient : IHealthChecker
{
    [Get("Providers/{ukprn}")]
    [AllowAnyStatusCode]
    Task<Response<GetProviderResponse>> GetProvider([Path] int ukprn, CancellationToken cancellationToken);

    [Patch("Providers/{ukprn}?userId={userId}&userDisplayName={userDisplayName}")]
    [AllowAnyStatusCode]
    Task<HttpResponseMessage> UpdateProviderDescription([Path] int ukprn, [Path] string userId, [Path] string userDisplayName, [Body] List<PatchOperation> patchOperations, CancellationToken cancellationToken);
}
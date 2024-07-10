using RestEase;
using SFA.DAS.RoatpOversight.Application.Commands.CreateProvider;

namespace SFA.DAS.RoatpOversight.Infrastructure;
public interface IRoatpV2ApiClient : IHealthChecker
{
    [Post("Providers?userId={userId}&userDisplayName={userDisplayName}")]
    [AllowAnyStatusCode]
    Task<HttpResponseMessage> CreateProvider([Path] string userId, [Path] string userDisplayName, [Body] CreateProviderCommand command, CancellationToken cancellationToken);
}
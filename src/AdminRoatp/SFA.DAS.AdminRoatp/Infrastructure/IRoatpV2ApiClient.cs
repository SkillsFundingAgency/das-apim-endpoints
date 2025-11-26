using RestEase;
using SFA.DAS.AdminRoatp.Application.Commands.CreateProvider;

namespace SFA.DAS.AdminRoatp.Infrastructure;
public interface IRoatpV2ApiClient
{
    [Post("Providers?userId={userId}&userDisplayName={userDisplayName}")]
    [AllowAnyStatusCode]
    Task<HttpResponseMessage> CreateProvider([Path] string userId, [Path] string userDisplayName, [Body] CreateProviderModel model, CancellationToken cancellationToken);
}
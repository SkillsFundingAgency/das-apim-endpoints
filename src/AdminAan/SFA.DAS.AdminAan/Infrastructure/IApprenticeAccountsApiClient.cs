using RestEase;
using SFA.DAS.AdminAan.Domain.ApprenticeAccount;

namespace SFA.DAS.AdminAan.Infrastructure;

public interface IApprenticeAccountsApiClient
{
    [Get("apprentices/{id}/MyApprenticeship")]
    [AllowAnyStatusCode]
    Task<Response<GetMyApprenticeshipResponse>> GetMyApprenticeship([Path] Guid id, CancellationToken cancellationToken);
}

using RestEase;
using SFA.DAS.EmployerAan.InnerApi.MyApprenticeships;

namespace SFA.DAS.EmployerAan.Infrastructure;
public interface IApprenticeAccountsApiClient : IHealthChecker
{
    [Get("apprentices/{apprenticeId}/MyApprenticeship")]
    [AllowAnyStatusCode]
    Task<Response<GetMyApprenticeshipResponse>> GetMyApprenticeship([Path] Guid apprenticeId, CancellationToken cancellationToken);
}

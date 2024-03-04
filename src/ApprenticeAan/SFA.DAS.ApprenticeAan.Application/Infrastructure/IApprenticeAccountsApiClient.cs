using RestEase;
using SFA.DAS.ApprenticeAan.Application.ApprenticeAccount.Queries.GetApprenticeAccount;
using SFA.DAS.ApprenticeAan.Application.InnerApi.MyApprenticeships;
using SFA.DAS.ApprenticeAan.Application.MyApprenticeships.Commands.CreateMyApprenticeship;

namespace SFA.DAS.ApprenticeAan.Application.Infrastructure;
public interface IApprenticeAccountsApiClient : IHealthChecker
{
    [Get("apprentices/{apprenticeId}")]
    [AllowAnyStatusCode]
    Task<Response<GetApprenticeAccountQueryResult>> GetApprentice([Path] Guid apprenticeId, CancellationToken cancellationToken);


    [Get("apprentices/{apprenticeId}/MyApprenticeship")]
    [AllowAnyStatusCode]
    Task<Response<GetMyApprenticeshipResponse>> GetMyApprenticeship([Path] Guid apprenticeId, CancellationToken cancellationToken);

    [Post("apprentices/{apprenticeId}/MyApprenticeship")]
    [AllowAnyStatusCode]
    Task<HttpResponseMessage> PostMyApprenticeship(
        [Path] Guid apprenticeId,
        [Body] CreateMyApprenticeshipCommand command,
        CancellationToken cancellationToken);
}
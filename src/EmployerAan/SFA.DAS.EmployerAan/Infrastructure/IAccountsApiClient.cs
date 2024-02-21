using RestEase;
using SFA.DAS.EmployerAan.Application.User.Commands;
using SFA.DAS.EmployerAan.Application.User.GetUserAccounts;

namespace SFA.DAS.EmployerAan.Infrastructure;
public interface IAccountsApiClient : IHealthChecker
{
    [Get("api/user/{id}/accounts")]
    [AllowAnyStatusCode]
    Task<Response<List<GetUserAccountsResponse>>> GetUserAccounts([Path] string id, CancellationToken cancellationToken);

    [Get("api/accounts/{accountId}/users")]
    [AllowAnyStatusCode]
    Task<Response<List<GetAccountTeamMembersResponse>>> GetAccountTeamMembers([Path] string accountId, CancellationToken cancellationToken);

    [Put("api/user/upsert")]
    [AllowAnyStatusCode]
    Task PutAccountUser(PutAccountUserRequest request, CancellationToken cancellationToken);
}

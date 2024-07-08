using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests;

public class GetAccountTeamMembersRequest : IGetAllApiRequest
{
    private readonly long _accountId;

    public GetAccountTeamMembersRequest(long accountId) => _accountId = accountId;

    public string GetAllUrl => $"api/accounts/internal/{_accountId}/users";
}
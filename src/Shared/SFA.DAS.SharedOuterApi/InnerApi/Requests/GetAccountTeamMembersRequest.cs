using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests;

public class GetAccountTeamMembersRequest : IGetAllApiRequest
{
    private readonly string _hashedAccountId;
    private readonly long _accountId;

    public GetAccountTeamMembersRequest(string hashedAccountId) => _hashedAccountId = hashedAccountId;

    public GetAccountTeamMembersRequest(long accountId) => _accountId = accountId;

    public string GetAllUrl => !string.IsNullOrEmpty(_hashedAccountId)
        ? $"api/accounts/{_hashedAccountId}/users"
        : $"api/accounts/internal/{_accountId}/users";
}
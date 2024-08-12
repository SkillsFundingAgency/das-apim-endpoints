using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests;

public class GetAccountTeamMembersByInternalAccountIdRequest : IGetAllApiRequest
{
    private readonly long _accountId;

    public GetAccountTeamMembersByInternalAccountIdRequest(long accountId)
    {
        _accountId = accountId;
    }

    public string GetAllUrl => $"api/accounts/internal/{_accountId}/users";
}

using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests;

public class GetAccountTeamMembersByInternalAccountIdRequest : IGetAllApiRequest, IGetApiRequest
{
    private readonly long _accountId;

    public GetAccountTeamMembersByInternalAccountIdRequest(long accountId)
    {
        _accountId = accountId;
    }

    public string GetAllUrl => $"api/accounts/internal/{_accountId}/users";

    public string GetUrl => $"api/accounts/internal/{_accountId}/users";
}

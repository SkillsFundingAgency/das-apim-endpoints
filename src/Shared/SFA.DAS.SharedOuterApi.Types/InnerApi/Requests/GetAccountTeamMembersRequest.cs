using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;

public class GetAccountTeamMembersRequest : IGetAllApiRequest
{
    private readonly long _accountId;

    public GetAccountTeamMembersRequest(long accountId) => _accountId = accountId;

    public string GetAllUrl => $"api/accounts/internal/{_accountId}/users";
}
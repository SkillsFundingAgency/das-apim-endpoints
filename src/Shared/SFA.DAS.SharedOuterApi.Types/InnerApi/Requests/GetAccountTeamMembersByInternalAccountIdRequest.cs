using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;

public class GetAccountTeamMembersByInternalAccountIdRequest(long accountId) : IGetAllApiRequest, IGetApiRequest
{
    public string GetAllUrl => $"api/accounts/internal/{accountId}/users";

    public string GetUrl => $"api/accounts/internal/{accountId}/users";
}

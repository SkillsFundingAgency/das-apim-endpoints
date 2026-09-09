using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;

public class GetAccountTeamMembersRequest(long accountId) : IGetAllApiRequest
{
    public string GetAllUrl => $"api/accounts/internal/{accountId}/users";
}
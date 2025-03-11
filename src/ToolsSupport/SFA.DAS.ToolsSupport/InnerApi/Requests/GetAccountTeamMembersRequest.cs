using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetAccountTeamMembersRequest(long id) : IGetApiRequest
{
    public string GetUrl => $"api/accounts/internal/{id}/users";
}

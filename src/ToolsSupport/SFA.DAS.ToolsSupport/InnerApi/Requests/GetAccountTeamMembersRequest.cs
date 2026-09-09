using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetAccountTeamMembersRequest(long id) : IGetApiRequest
{
    public string GetUrl => $"api/accounts/internal/{id}/users";
}

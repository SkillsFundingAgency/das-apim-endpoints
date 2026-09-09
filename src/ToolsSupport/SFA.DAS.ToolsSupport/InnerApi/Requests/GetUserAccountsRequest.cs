using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetUserAccountsRequest(Guid userId) : IGetApiRequest
{
    public string GetUrl => $"api/user/{userId}/accounts";
}

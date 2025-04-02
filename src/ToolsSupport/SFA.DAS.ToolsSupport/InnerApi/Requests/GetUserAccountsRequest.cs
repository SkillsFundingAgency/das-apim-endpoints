using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetUserAccountsRequest(Guid userId) : IGetApiRequest
{
    public string GetUrl => $"api/user/{userId}/accounts";
}

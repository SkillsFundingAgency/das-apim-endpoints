using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests.Account;

public class GetAccountRequest(long accountId) : IGetApiRequest
{
    public string GetUrl => $"api/accounts/{accountId}";
}

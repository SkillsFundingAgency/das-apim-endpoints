using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetAccountBalancesRequest(List<string> accountIds) : IPostApiRequest
{
    public string PostUrl => "api/accounts/balances";
    public object Data { get; set; } = accountIds;
}
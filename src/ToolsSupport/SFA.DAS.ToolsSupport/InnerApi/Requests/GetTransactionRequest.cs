using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetTransactionRequest(string accountId, int year, int month) : IGetApiRequest
{
    public string GetUrl => $"api/accounts/{accountId}/transactions/{year}/{month}";
}
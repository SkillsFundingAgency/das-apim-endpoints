using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetAllTransactionsRequest(string accountId, int year, int month) : IGetApiRequest
{
    public string GetUrl => $"api/accounts/{accountId}/transactions/all-transactions/{year}/{month}";
}
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetAllTransactionsRequest(string accountId, DateTime fromdate, DateTime toDate) : IGetApiRequest
{
    public string GetUrl => $"api/accounts/{accountId}/transactions/query?fromDate={fromdate:yyyy-MM-dd}&toDate={toDate:yyyy-MM-dd}";
}
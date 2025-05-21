using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.InnerApi.Responses;

namespace SFA.DAS.ToolsSupport.Interfaces;

public interface IEmployerFinanceService
{
    Task<ApiResponse<List<AccountBalance>>> GetAccountBalances(GetAccountBalancesRequest request);
    Task<TransactionsViewModel> GetAllTransactions(string accountId, DateTime fromdate, DateTime toDate);
}

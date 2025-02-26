using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using SFA.DAS.ToolsSupport.Interfaces;

namespace SFA.DAS.ToolsSupport.Application.Services;

public class EmployerFinanceService(IFinanceApiClient<FinanceApiConfiguration> client) : IEmployerFinanceService
{
    public async Task<ApiResponse<List<AccountBalance>>> GetAccountBalances(GetAccountBalancesRequest request)
    {
        return await client.PostWithResponseCode<List<AccountBalance>>(request, false);
    }

    public async Task<TransactionsViewModel> GetAllTransactions(string accountId, int year, int month)
    {
        var response = await client.Get<TransactionsViewModel>(new GetAllTransactionsRequest(accountId, year, month));

        return response;
    }
}

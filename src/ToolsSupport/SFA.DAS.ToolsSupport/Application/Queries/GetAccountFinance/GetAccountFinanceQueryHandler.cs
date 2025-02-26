using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ToolsSupport.Application.Services;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using SFA.DAS.ToolsSupport.Interfaces;

namespace SFA.DAS.ToolsSupport.Application.Queries.GetAccountFinance;

public class GetAccountFinanceQueryHandler(
        IAccountsService accountsService,    
    IFinanceDataService financeDataService,
    ILogger<GetAccountFinanceQueryHandler> logger)
        : IRequestHandler<GetAccountFinanceQuery, GetAccountFinanceQueryResult>
{
    public async Task<GetAccountFinanceQueryResult> Handle(GetAccountFinanceQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting Finance data for Account {account}", query.AccountId);

        var account = await accountsService.GetAccount(query.AccountId);
        if (account != null)
        {
            return await financeDataService.GetFinanceData(account);
        }

        return new GetAccountFinanceQueryResult();
    }

   
}
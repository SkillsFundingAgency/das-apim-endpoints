using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ToolsSupport.Application.Services;
using SFA.DAS.ToolsSupport.Interfaces;

namespace SFA.DAS.ToolsSupport.Application.Queries.GetChallenge;

public class GetChallengeQueryHandler(
    IAccountsService accountsService,
    IChallengeService challengeService,
    IFinanceDataService financeDataService,
    ILogger<GetChallengeQueryHandler> logger)
        : IRequestHandler<GetChallengeQuery, GetChallengeQueryResult>
{
    public async Task<GetChallengeQueryResult> Handle(GetChallengeQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting Challenge Data for Account {account}", query.AccountId);

        var account = await accountsService.GetAccount(query.AccountId);

        var financeData = await financeDataService.GetFinanceData(account);

        return challengeService.GetChallengeQueryResultFromAccount(account, financeData.PayeSchemes);
    }
}
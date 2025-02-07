using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using SFA.DAS.ToolsSupport.Interfaces;
using SFA.DAS.ToolsSupport.Strategies;

namespace SFA.DAS.ToolsSupport.Application.Queries.EmployerAccount.GetEmployerAccountDetails;

public class GetEmployerAccountDetailsQueryHandler(
    IAccountDetailsStrategyFactory strategyFactory,
    IAccountsService accountsService,
    ILogger<GetEmployerAccountDetailsQueryHandler> logger)
        : IRequestHandler<GetEmployerAccountDetailsQuery, GetEmployerAccountDetailsResult>
{
    public async Task<GetEmployerAccountDetailsResult> Handle(GetEmployerAccountDetailsQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting Account Details for Account {account}", query.AccountId);

        // get account details
        var account = await accountsService.GetAccount(query.AccountId);
        if (account != null)
        {
            // populate Ienumerable
            var accountDetailsStrategy = strategyFactory.CreateStrategy(query.SelectedField);
            var result = await accountDetailsStrategy.ExecuteAsync(account);

            result = MapCommonAccountDetails(account, result);

            return result;
        }
        return new GetEmployerAccountDetailsResult();
    }

    private static GetEmployerAccountDetailsResult MapCommonAccountDetails(Account account, GetEmployerAccountDetailsResult result )
    {
        result.AccountId = account.AccountId;
        result.HashedAccountId = account.HashedAccountId;
        result.PublicHashedAccountId = account.PublicHashedAccountId;
        result.DasAccountName = account.DasAccountName;
        result.DateRegistered = account.DateRegistered;
        result.OwnerEmail = account.OwnerEmail;
        result.ApprenticeshipEmployerType = account.ApprenticeshipEmployerType;
        return result;
    }
}

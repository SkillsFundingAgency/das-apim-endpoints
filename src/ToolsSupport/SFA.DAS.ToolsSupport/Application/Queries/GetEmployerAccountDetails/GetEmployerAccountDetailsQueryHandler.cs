using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ToolsSupport.Interfaces;

namespace SFA.DAS.ToolsSupport.Application.Queries.GetEmployerAccountDetails;

public class GetEmployerAccountDetailsQueryHandler(
    IAccountsService accountsService,
    ILogger<GetEmployerAccountDetailsQueryHandler> logger)
        : IRequestHandler<GetEmployerAccountDetailsQuery, GetEmployerAccountDetailsResult>
{
    public async Task<GetEmployerAccountDetailsResult> Handle(GetEmployerAccountDetailsQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting Account Details for Account {account}", query.AccountId);

        var account = await accountsService.GetAccount(query.AccountId);
        if (account != null)
        {
            return new GetEmployerAccountDetailsResult
            {
                AccountId = account.AccountId,
                HashedAccountId = account.HashedAccountId,
                PublicHashedAccountId = account.PublicHashedAccountId,
                DasAccountName = account.DasAccountName,
                DateRegistered = account.DateRegistered,
                OwnerEmail = account.OwnerEmail,
                ApprenticeshipEmployerType = account.ApprenticeshipEmployerType
            };
        }
        return new GetEmployerAccountDetailsResult();
    }
}

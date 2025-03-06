using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ToolsSupport.Interfaces;

namespace SFA.DAS.ToolsSupport.Application.Queries.GetAccountOrganisations;

public class GetAccountOrganisationsQueryHandler(
    IAccountsService accountsService,
    ILogger<GetAccountOrganisationsQueryHandler> logger)
        : IRequestHandler<GetAccountOrganisationsQuery, GetAccountOrganisationsQueryResult>
{
    public async Task<GetAccountOrganisationsQueryResult> Handle(GetAccountOrganisationsQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting Organisations for Account {account}", query.AccountId);

        var account = await accountsService.GetAccount(query.AccountId);
        if (account != null)
        {
            var legalEntities = account.LegalEntities;

            var tasks = legalEntities.Select(entity => accountsService.GetEmployerAccountLegalEntity(entity.Href));
            var legalEntitiesList = await Task.WhenAll(tasks);
            return new GetAccountOrganisationsQueryResult
            {
                LegalEntities = legalEntitiesList
            };
        }

        return new GetAccountOrganisationsQueryResult();
    }
}
using SFA.DAS.ToolsSupport.Application.Queries.GetEmployerAccountDetails;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using SFA.DAS.ToolsSupport.Interfaces;

namespace SFA.DAS.ToolsSupport.Strategies;

public class AccountDetailsLegalEntitiesStrategy(IAccountsService accountsService) : IAccountDetailsStrategy
{
    public async Task<GetEmployerAccountDetailsResult> ExecuteAsync(Account account)
    {
        var legalEntities = account.LegalEntities;

       var tasks = legalEntities.Select(entity => accountsService.GetEmployerAccountLegalEntity(entity.Href));
        var legalEntitiesList = await Task.WhenAll(tasks);
        return new GetEmployerAccountDetailsResult
        {
            LegalEntities = legalEntitiesList
        };
    }
}

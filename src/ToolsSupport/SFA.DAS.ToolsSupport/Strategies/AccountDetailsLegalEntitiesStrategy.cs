using Microsoft.Extensions.Logging;
using SFA.DAS.ToolsSupport.Application.Queries.EmployerAccount.GetEmployerAccountDetails;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using SFA.DAS.ToolsSupport.Interfaces;

namespace SFA.DAS.ToolsSupport.Strategies;

public class AccountDetailsLegalEntitiesStrategy(IAccountsService accountsService) : IAccountDetailsStrategy
{
    public async Task<GetEmployerAccountDetailsResult> ExecuteAsync(Account account)
    {
        var legalEntitiesList = new List<LegalEntity>();

        var legalEntities = account.LegalEntities;

        foreach (var entity in legalEntities)
        {
            var legalResponse = await accountsService.GetEmployerAccountLegalEntity(entity.Href);
            legalEntitiesList.Add(legalResponse);
        }

        return new GetEmployerAccountDetailsResult
        {          
            LegalEntities = legalEntitiesList
        };
    }
}

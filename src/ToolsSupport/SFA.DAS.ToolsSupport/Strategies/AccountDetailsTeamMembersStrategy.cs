using SFA.DAS.ToolsSupport.Application.Queries.GetEmployerAccountDetails;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using SFA.DAS.ToolsSupport.Interfaces;

namespace SFA.DAS.ToolsSupport.Strategies;

public class AccountDetailsTeamMembersStrategy(IAccountsService accountsService) : IAccountDetailsStrategy
{
    public async Task<GetEmployerAccountDetailsResult> ExecuteAsync(Account account)
    {
        var teamMembers = await accountsService.GetAccountTeamMembers(account.AccountId);

        return new GetEmployerAccountDetailsResult
        {
            TeamMembers = teamMembers ?? []
        };
    }
}

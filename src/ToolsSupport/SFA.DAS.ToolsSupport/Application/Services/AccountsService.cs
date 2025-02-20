using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.ToolsSupport.InnerApi.Requests.Account;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using SFA.DAS.ToolsSupport.Interfaces;

namespace SFA.DAS.ToolsSupport.Application.Services;

public class AccountsService(IAccountsApiClient<AccountsConfiguration> client) : IAccountsService
{
    public async Task<Account> GetAccount(long accountId)
    {
        var response = await client.Get<Account>(new GetAccountRequest(accountId));

        return response;
    }

    public async Task<LegalEntity> GetEmployerAccountLegalEntity(string url)
    {
        var response = await client.Get<LegalEntity>(new GetEmployerAccountLegalEntityRequest(url));

        return response;
    }
}

using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Accounts;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly IAccountsApiClient<AccountsConfiguration> _client;

        public AccountsService(IAccountsApiClient<AccountsConfiguration> client)
        {
            _client = client;
        }

        public async Task<Account> GetAccount(string encodedAccountId)
        {
            var response = await _client.Get<Account>(new GetAccountRequest(encodedAccountId));

            return response;
        }

        public async Task<List<TeamMember>> GetAccountUsers(long accountId)
        {
            var response = await _client.Get<List<TeamMember>>(new GetAccountUsersRequest(accountId));
            return response;
        }
    }
}

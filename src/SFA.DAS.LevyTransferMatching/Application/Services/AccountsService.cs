using System;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Accounts;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
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
            var response = await _client.GetWithResponseCode<Account>(new GetAccountRequest(encodedAccountId));

            if (!((int) response.StatusCode >= 200 && (int) response.StatusCode <= 299))
            {
                throw new InvalidOperationException($"Error getting account from AccountsApi: {response.ErrorContent}");
            }

            return response.Body;
        }
    }
}

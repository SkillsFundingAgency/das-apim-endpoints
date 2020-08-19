using System;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.Accounts;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Accounts;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly IAccountsApiClient<AccountsConfiguration> _client;

        public AccountsService(IAccountsApiClient<AccountsConfiguration> client)
        {
            _client = client;
        }

        public Task<bool> IsHealthy()
        {
            throw new NotImplementedException();
        }

        public async Task<LegalEntity> GetLegalEntity(string accountId, long legalEntityId)
        {
            var response = await _client.Get<LegalEntity>(new GetLegalEntityRequest(accountId, legalEntityId));

            return response;
        }
    }
}

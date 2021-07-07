using System;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.Accounts;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Accounts;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

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

        public async Task<PagedResponse<AccountLegalEntity>> GetLegalEntitiesByPage(int pageNumber, int pageSize = 1000)
        {
            var response = await _client.GetPaged<AccountLegalEntity>(new GetPagedLegalEntitiesRequest(pageNumber, pageSize));

            return response;
        }
    }
}

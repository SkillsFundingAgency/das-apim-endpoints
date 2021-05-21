﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.Accounts;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.EmployerIncentives.Models;
using GetLegalEntityRequest = SFA.DAS.EmployerIncentives.InnerApi.Requests.GetLegalEntityRequest;

namespace SFA.DAS.EmployerIncentives.Application.Services
{
    public class LegalEntitiesService : ILegalEntitiesService
    {
        private readonly IEmployerIncentivesApiClient<EmployerIncentivesConfiguration> _client;

        public LegalEntitiesService(IEmployerIncentivesApiClient<EmployerIncentivesConfiguration> client)
        {
            _client = client;
        }

        public async Task<AccountLegalEntity[]> GetAccountLegalEntities(long accountId)
        {
            var response = await _client.GetAll<AccountLegalEntity>(new GetAccountLegalEntitiesRequest(accountId));

            return response.ToArray();
        }
        
        public async Task<AccountLegalEntity> GetLegalEntity(long accountId, long accountLegalEntityId)
        {
            var response = await _client.Get<AccountLegalEntity>(new GetLegalEntityRequest(accountId, accountLegalEntityId));

            return response;
        }

        public async Task DeleteAccountLegalEntity(long accountId, long accountLegalEntityId)
        {
            await _client.Delete(new DeleteAccountLegalEntityRequest(accountId, accountLegalEntityId));
        }
        
        public async Task CreateLegalEntity(long accountId, AccountLegalEntityCreateRequest accountLegalEntity)
        {
            var request = new PutAccountLegalEntityRequest(accountId) { Data = accountLegalEntity };
            await _client.Put(request);
        }
        
        public async Task RefreshLegalEntities(IEnumerable<InnerApi.Responses.Accounts.AccountLegalEntity> accountLegalEntities, int pageNumber, int pageSize, int totalPages)
        {
            var accountLegalEntitiesData = new Dictionary<string, object>
            {
                { "AccountLegalEntities", accountLegalEntities },
                { "PageNumber", pageNumber },
                { "PageSize", pageSize },
                { "TotalPages", totalPages }
            };
            var request = new RefreshLegalEntitiesRequestData { Type = JobType.RefreshLegalEntities, Data = accountLegalEntitiesData };
            await _client.Put(new RefreshLegalEntitiesRequest { Data = request });
        }

        public async Task SignAgreement(long accountId, long accountLegalEntityId, SignAgreementRequest request)
        {
            await _client.Patch(new PatchSignAgreementRequest(accountId, accountLegalEntityId) { Data = request });
        }
    }
}

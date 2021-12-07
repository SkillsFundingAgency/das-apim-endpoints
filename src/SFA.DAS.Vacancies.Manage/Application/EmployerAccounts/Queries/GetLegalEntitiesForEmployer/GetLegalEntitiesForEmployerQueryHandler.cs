using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Vacancies.Manage.InnerApi.Requests;
using SFA.DAS.Vacancies.Manage.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Manage.Application.EmployerAccounts.Queries.GetLegalEntitiesForEmployer
{
    public class GetLegalEntitiesForEmployerQueryHandler : IRequestHandler<GetLegalEntitiesForEmployerQuery, GetLegalEntitiesForEmployerResult>
    {
        private readonly IAccountsApiClient<AccountsConfiguration> _accountsApiClient;

        public GetLegalEntitiesForEmployerQueryHandler(IAccountsApiClient<AccountsConfiguration> accountsApiClient)
        {
            _accountsApiClient = accountsApiClient;
        }
        
        public async Task<GetLegalEntitiesForEmployerResult> Handle(GetLegalEntitiesForEmployerQuery request, CancellationToken cancellationToken)
        {
            var resourceListResponse = await _accountsApiClient.Get<AccountDetail>(
                new GetAllEmployerAccountLegalEntitiesRequest(request.EncodedAccountId));

            var legalEntities = new List<GetEmployerAccountLegalEntityItem>();
            
            if (resourceListResponse == null)
            {
                return new GetLegalEntitiesForEmployerResult
                {
                    LegalEntities = legalEntities
                };
            }

            foreach (var resource in resourceListResponse.LegalEntities)
            {
                var accountLegalEntityItem = await _accountsApiClient.Get<GetEmployerAccountLegalEntityItem>(
                    new GetEmployerAccountLegalEntityRequest(resource.Href));

                if (accountLegalEntityItem.Agreements.Any(c => c.Status == EmployerAgreementStatus.Signed))
                {
                    accountLegalEntityItem.AccountPublicHashedId = request.EncodedAccountId;
                    accountLegalEntityItem.AccountName = resourceListResponse.DasAccountName;
                    legalEntities.Add(accountLegalEntityItem);    
                }
                
            }
            
            return new GetLegalEntitiesForEmployerResult
            {
                LegalEntities = legalEntities
            };
        }
    }
}
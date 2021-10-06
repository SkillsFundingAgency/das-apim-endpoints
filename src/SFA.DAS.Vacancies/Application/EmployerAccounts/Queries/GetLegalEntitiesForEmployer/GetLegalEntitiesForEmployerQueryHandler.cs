using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Vacancies.InnerApi.Requests;
using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Application.EmployerAccounts.Queries.GetLegalEntitiesForEmployer
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
            var resourceListResponse = await _accountsApiClient.GetAll<Resource>(
                new GetAllEmployerAccountLegalEntitiesRequest(request.EncodedAccountId));

            var legalEntities = new List<GetEmployerAccountLegalEntityItem>();
            foreach (var resource in resourceListResponse)
            {
                var accountLegalEntityItem = await _accountsApiClient.Get<GetEmployerAccountLegalEntityItem>(
                    new GetEmployerAccountLegalEntityRequest(resource.Href));
                legalEntities.Add(accountLegalEntityItem);
            }
            
            return new GetLegalEntitiesForEmployerResult
            {
                LegalEntities = legalEntities
            };
        }
    }
}
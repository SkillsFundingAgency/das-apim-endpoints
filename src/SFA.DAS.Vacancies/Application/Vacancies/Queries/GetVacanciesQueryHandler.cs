using System;
using System.Linq;
using System.Security;
using MediatR;
using SFA.DAS.Vacancies.Configuration;
using SFA.DAS.Vacancies.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Vacancies.InnerApi.Requests;
using SFA.DAS.Vacancies.InnerApi.Responses;


namespace SFA.DAS.Vacancies.Application.Vacancies.Queries
{
    public class GetVacanciesQueryHandler: IRequestHandler<GetVacanciesQuery, GetVacanciesQueryResult>
    {
        private readonly IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> _findApprenticeshipApiClient;
        private readonly IAccountsApiClient<AccountsConfiguration> _accountsApiClient;
        private readonly IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> _apiClient;

        public GetVacanciesQueryHandler(IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient, IAccountsApiClient<AccountsConfiguration> accountsApiClient, IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> apiClient)
        {
            _findApprenticeshipApiClient = findApprenticeshipApiClient;
            _accountsApiClient = accountsApiClient;
            _apiClient = apiClient;
        }

        public async Task<GetVacanciesQueryResult> Handle(GetVacanciesQuery request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.AccountLegalEntityPublicHashedId))
            {
                if (request.Ukprn == null && string.IsNullOrEmpty(request.AccountPublicHashedId))
                {
                    throw new SecurityException();
                }
                
                if (request.Ukprn != null && string.IsNullOrEmpty(request.AccountPublicHashedId))
                {
                    var providerResponse =
                        await _apiClient.Get<GetProviderAccountLegalEntitiesResponse>(
                            new GetProviderAccountLegalEntitiesRequest(request.Ukprn));
                    if (!providerResponse.AccountProviderLegalEntities.Select(c => c.AccountLegalEntityPublicHashedId).Contains(request.AccountLegalEntityPublicHashedId, StringComparer.CurrentCultureIgnoreCase))
                    {
                        throw new SecurityException();
                    }
                }
                
                if (!string.IsNullOrEmpty(request.AccountPublicHashedId))
                {
                    var resourceListResponse = await _accountsApiClient.Get<AccountDetail>(
                        new GetAllEmployerAccountLegalEntitiesRequest(request.AccountPublicHashedId));
                    var isInAccount = false;
                    foreach (var legalEntity in resourceListResponse.LegalEntities)
                    {
                        var legalEntityResponse =
                            await _accountsApiClient.Get<GetEmployerAccountLegalEntityItem>(
                                new GetEmployerAccountLegalEntityRequest(legalEntity.Href));
                        
                        if (legalEntityResponse.AccountLegalEntityPublicHashedId.Equals(request.AccountLegalEntityPublicHashedId, StringComparison.CurrentCultureIgnoreCase))
                        {
                            isInAccount = true;
                            break;
                        }
                    }
                    if (!isInAccount)
                    {
                        throw new SecurityException();
                    }
                }
            }

            var response = await _findApprenticeshipApiClient.Get<GetVacanciesResponse>(new GetVacanciesRequest(request.PageNumber, request.PageSize, request.AccountLegalEntityPublicHashedId, request.Ukprn, request.AccountPublicHashedId));

            return new GetVacanciesQueryResult()
            {
                Vacancies = response.ApprenticeshipVacancies
            };
        }
    }
}
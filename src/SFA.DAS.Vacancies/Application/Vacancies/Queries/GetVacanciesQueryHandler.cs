using System.Linq;
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

        public GetVacanciesQueryHandler(IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient)
        {
            _findApprenticeshipApiClient = findApprenticeshipApiClient;
        }

        public GetVacanciesQueryHandler(IAccountsApiClient<AccountsConfiguration> accountsApiClient)
        {
            _accountsApiClient = accountsApiClient;
        }

        public GetVacanciesQueryHandler(IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetVacanciesQueryResult> Handle(GetVacanciesQuery request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.AccountLegalEntityPublicHashedId))
            {
                if (request.AccountType == AccountType.Provider)
                {
                    var providerResponse =
                        await _apiClient.Get<GetProviderAccountLegalEntitiesResponse>(
                            new GetProviderAccountLegalEntitiesRequest(request.Ukprn));
                    if (!providerResponse.AccountProviderLegalEntities.Select(c => c.AccountLegalEntityPublicHashedId).Contains(request.AccountLegalEntityPublicHashedId))
                    {
                        //throw somesecurity exception 
                    }
                }
                else if (request.AccountType == AccountType.Employer)
                {
                    var resourceListResponse = await _accountsApiClient.Get<AccountDetail>(
                        new GetAllEmployerAccountLegalEntitiesRequest(request.EncodedAccountId));
                    if (!resourceListResponse.PublicHashedAccountId.Select(c => c.Id).Contains(request.AccountLegalEntityPublicHashedId))
                    {
                        //throw some security exception 
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
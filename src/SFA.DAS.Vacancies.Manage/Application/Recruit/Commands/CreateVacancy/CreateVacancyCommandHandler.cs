using System;
using System.Linq;
using System.Net;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Vacancies.Manage.Configuration;
using SFA.DAS.Vacancies.Manage.InnerApi.Requests;
using SFA.DAS.Vacancies.Manage.InnerApi.Responses;
using SFA.DAS.Vacancies.Manage.Interfaces;

namespace SFA.DAS.Vacancies.Manage.Application.Recruit.Commands.CreateVacancy
{
    public class CreateVacancyCommandHandler : IRequestHandler<CreateVacancyCommand, CreateVacancyCommandResponse>
    {
        private readonly IRecruitApiClient<RecruitApiConfiguration> _recruitApiClient;
        private readonly IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> _providerRelationshipsApiClient;
        private readonly IAccountsApiClient<AccountsConfiguration> _accountsApiClient;

        public CreateVacancyCommandHandler (IRecruitApiClient<RecruitApiConfiguration> recruitApiClient, 
            IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> providerRelationshipsApiClient,
            IAccountsApiClient<AccountsConfiguration> accountsApiClient)
        {
            _recruitApiClient = recruitApiClient;
            _providerRelationshipsApiClient = providerRelationshipsApiClient;
            _accountsApiClient = accountsApiClient;
        }
        public async Task<CreateVacancyCommandResponse> Handle(CreateVacancyCommand request, CancellationToken cancellationToken)
        {
            switch (request.PostVacancyRequestData.OwnerType)
            {
                case OwnerType.Provider:
                    var providerResponse =
                        await _providerRelationshipsApiClient.Get<GetProviderAccountLegalEntitiesResponse>(
                            new GetProviderAccountLegalEntitiesRequest(request.PostVacancyRequestData.User.Ukprn));
                    var legalEntityItem = providerResponse.AccountProviderLegalEntities
                        .FirstOrDefault(c => c.AccountLegalEntityPublicHashedId.Equals(
                            request.PostVacancyRequestData.AccountLegalEntityPublicHashedId, StringComparison.CurrentCultureIgnoreCase));
                    if (legalEntityItem != null)
                    {
                        request.PostVacancyRequestData.LegalEntityName = legalEntityItem.AccountLegalEntityName;
                    }
                    else
                    {
                        throw new SecurityException();
                    }
                    break;
                case OwnerType.Employer:
                    var resourceListResponse = await _accountsApiClient.Get<AccountDetail>(
                        new GetAllEmployerAccountLegalEntitiesRequest(request.PostVacancyRequestData.EmployerAccountId));
                    var isInAccount = false;
                    var accountLegalEntityName = "";
                    foreach (var legalEntity in resourceListResponse.LegalEntities)
                    {
                        var legalEntityResponse =
                            await _accountsApiClient.Get<GetEmployerAccountLegalEntityItem>(
                                new GetEmployerAccountLegalEntityRequest(legalEntity.Href));
                        
                        if (legalEntityResponse.AccountLegalEntityPublicHashedId.Equals(request.PostVacancyRequestData.AccountLegalEntityPublicHashedId, StringComparison.CurrentCultureIgnoreCase))
                        {
                            isInAccount = true;
                            accountLegalEntityName = legalEntityResponse.AccountLegalEntityName;
                            break;
                        }
                    }

                    if (isInAccount)
                    {
                        request.PostVacancyRequestData.LegalEntityName = accountLegalEntityName;    
                    }
                    else
                    {
                        throw new SecurityException();
                    }
                    
                    break;
            }
            
            var apiRequest = new PostVacancyRequest(request.Id, request.PostVacancyRequestData);

            var result = await _recruitApiClient.PostWithResponseCode<string>(apiRequest);

            if(!((int)result.StatusCode >= 200 && (int)result.StatusCode <= 299))
            {
                if (result.StatusCode.Equals(HttpStatusCode.BadRequest))
                {
                    throw new HttpRequestContentException($"Response status code does not indicate success: {(int)result.StatusCode} ({result.StatusCode})", result.StatusCode, result.ErrorContent);    
                }

                throw new Exception(
                    $"Response status code does not indicate success: {(int) result.StatusCode} ({result.StatusCode})");
            }
            
            return new CreateVacancyCommandResponse
            {
                VacancyReference = result.Body
            };
        }
    }
}
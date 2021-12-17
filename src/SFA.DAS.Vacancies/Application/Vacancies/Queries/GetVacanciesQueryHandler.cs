using System.Security;
using MediatR;
using SFA.DAS.Vacancies.Configuration;
using SFA.DAS.Vacancies.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Vacancies.InnerApi.Requests;
using SFA.DAS.Vacancies.InnerApi.Responses;


namespace SFA.DAS.Vacancies.Application.Vacancies.Queries
{
    public class GetVacanciesQueryHandler: IRequestHandler<GetVacanciesQuery, GetVacanciesQueryResult>
    {
        private readonly IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> _findApprenticeshipApiClient;
        private readonly IAccountLegalEntityPermissionService _accountLegalEntityPermissionService;

        public GetVacanciesQueryHandler(IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient, IAccountLegalEntityPermissionService accountLegalEntityPermissionService)
        {
            _findApprenticeshipApiClient = findApprenticeshipApiClient;
            _accountLegalEntityPermissionService = accountLegalEntityPermissionService;
        }

        public async Task<GetVacanciesQueryResult> Handle(GetVacanciesQuery request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.AccountLegalEntityPublicHashedId))
            {
                switch (request.AccountIdentifier.AccountType)
                {
                    case AccountType.Unknown:
                        throw new SecurityException();
                    case AccountType.External:
                        request.AccountLegalEntityPublicHashedId = string.Empty;
                        request.AccountPublicHashedId = string.Empty;
                        break;
                    default:
                    {
                        var accountLegalEntity = await _accountLegalEntityPermissionService.GetAccountLegalEntity(request.AccountIdentifier,
                                request.AccountLegalEntityPublicHashedId);
                        
                        if (accountLegalEntity == null)
                        {
                            throw new SecurityException();
                        }
                        break;
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
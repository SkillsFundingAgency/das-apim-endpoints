using System;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Vacancies.Configuration;
using SFA.DAS.Vacancies.InnerApi.Requests;
using SFA.DAS.Vacancies.InnerApi.Responses;
using SFA.DAS.Vacancies.Interfaces;

namespace SFA.DAS.Vacancies.Application.Vacancies.Queries
{
    public class GetTraineeshipVacanciesQueryHandler : IRequestHandler<GetTraineeshipVacanciesQuery, GetTraineeshipVacanciesQueryResult>
    {
        private readonly IFindTraineeshipApiClient<FindTraineeshipApiConfiguration> _findTraineeshipApiClient;
        private readonly IAccountLegalEntityPermissionService _accountLegalEntityPermissionService;
        private readonly VacanciesConfiguration _vacanciesConfiguration;

        public GetTraineeshipVacanciesQueryHandler(IFindTraineeshipApiClient<FindTraineeshipApiConfiguration> findTraineeshipApiClient,
            IAccountLegalEntityPermissionService accountLegalEntityPermissionService,
            IOptions<VacanciesConfiguration> vacanciesConfiguration)
        {
            _findTraineeshipApiClient = findTraineeshipApiClient;
            _accountLegalEntityPermissionService = accountLegalEntityPermissionService;
            _vacanciesConfiguration = vacanciesConfiguration.Value;
        }

        public async Task<GetTraineeshipVacanciesQueryResult> Handle(GetTraineeshipVacanciesQuery request, CancellationToken cancellationToken)
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

            var vacanciesTask = _findTraineeshipApiClient.Get<GetTraineeshipVacanciesResponse>(new GetTraineeshipVacanciesRequest(
                request.PageNumber, request.PageSize, request.AccountLegalEntityPublicHashedId,
                request.Ukprn, request.AccountPublicHashedId, request.RouteIds, request.NationWideOnly,
                request.Lat, request.Lon, request.DistanceInMiles, request.PostedInLastNumberOfDays, request.Sort));

            await Task.WhenAll(vacanciesTask);

            foreach (var vacanciesItem in vacanciesTask.Result.TraineeshipVacancies)
            {
                vacanciesItem.VacancyUrl = $"{_vacanciesConfiguration.FindATraineeshipBaseUrl}/traineeship/reference/{vacanciesItem.VacancyReference}";
            }

            return new GetTraineeshipVacanciesQueryResult()
            {
                Vacancies = vacanciesTask.Result.TraineeshipVacancies,
                Total = vacanciesTask.Result.Total,
                TotalFiltered = vacanciesTask.Result.TotalFound,
                TotalPages = request.PageSize != 0 ? (int)Math.Ceiling((decimal)vacanciesTask.Result.TotalFound / request.PageSize) : 0
            };
        }
    }
}
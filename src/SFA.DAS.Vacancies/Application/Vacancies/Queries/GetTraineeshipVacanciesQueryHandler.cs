using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using MediatR;
using SFA.DAS.Vacancies.Configuration;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Vacancies.InnerApi.Responses;
using SFA.DAS.Vacancies.InnerApi.Requests;

namespace SFA.DAS.Vacancies.Application.Vacancies.Queries
{
    public class GetTraineeshipVacanciesQueryHandler : IRequestHandler<GetTraineeshipVacanciesQuery, GetTraineeshipVacanciesQueryResult>
    {
        private readonly IFindTraineeshipApiClient<FindTraineeshipApiConfiguration> _findTraineeshipApiClient;
        private readonly IAccountLegalEntityPermissionService _accountLegalEntityPermissionService;
        private readonly ICourseService _courseService;
        private readonly VacanciesConfiguration _vacanciesConfiguration;

        public GetTraineeshipVacanciesQueryHandler(IFindTraineeshipApiClient<FindTraineeshipApiConfiguration> findTraineeshipApiClient,
            IAccountLegalEntityPermissionService accountLegalEntityPermissionService,
            ICourseService courseService,
            IOptions<VacanciesConfiguration> vacanciesConfiguration)
        {
            _findTraineeshipApiClient = findTraineeshipApiClient;
            _accountLegalEntityPermissionService = accountLegalEntityPermissionService;
            _courseService = courseService;
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

            var categories = _courseService.MapRoutesToCategories(request.Routes);

            var vacanciesTask = _findTraineeshipApiClient.Get<GetTraineeshipVacanciesResponse>(new GetTraineeshipVacanciesRequest(
                request.PageNumber, request.PageSize, request.AccountLegalEntityPublicHashedId,
                request.Ukprn, request.AccountPublicHashedId, request.RouteId, request.NationWideOnly,
                request.Lat, request.Lon, request.DistanceInMiles, categories, request.PostedInLastNumberOfDays, request.Sort));

            await Task.WhenAll(vacanciesTask);

            foreach (var vacanciesItem in vacanciesTask.Result.TraineeshipVacancies)
            {
                if (vacanciesItem.RouteId == null)
                {
                    continue;
                }

                vacanciesItem.VacancyUrl = $"{_vacanciesConfiguration.FindATraineeshipBaseUrl}/traineeship/reference/{vacanciesItem.VacancyReference}";
            }

            return new GetTraineeshipVacanciesQueryResult()
            {
                Vacancies = vacanciesTask.Result.TraineeshipVacancies.Where(c => c.RouteId != null).ToList(),
                Total = vacanciesTask.Result.Total,
                TotalFiltered = vacanciesTask.Result.TotalFound,
                TotalPages = request.PageSize != 0 ? (int)Math.Ceiling((decimal)vacanciesTask.Result.TotalFound / request.PageSize) : 0
            };
        }
    }
}
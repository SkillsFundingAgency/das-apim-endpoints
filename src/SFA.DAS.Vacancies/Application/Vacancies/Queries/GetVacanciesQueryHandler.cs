using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using MediatR;
using SFA.DAS.Vacancies.Configuration;
using SFA.DAS.Vacancies.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
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
        private readonly IStandardsService _standardsService;
        private readonly VacanciesConfiguration _vacanciesConfiguration;

        public GetVacanciesQueryHandler(IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient, 
            IAccountLegalEntityPermissionService accountLegalEntityPermissionService, 
            IStandardsService standardsService,
            IOptions<VacanciesConfiguration> vacanciesConfiguration)
        {
            _findApprenticeshipApiClient = findApprenticeshipApiClient;
            _accountLegalEntityPermissionService = accountLegalEntityPermissionService;
            _standardsService = standardsService;
            _vacanciesConfiguration = vacanciesConfiguration.Value;
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

            var categories = _standardsService.MapRoutesToCategories(request.Routes);

            var vacanciesTask = _findApprenticeshipApiClient.Get<GetVacanciesResponse>(new GetVacanciesRequest(
                request.PageNumber, request.PageSize, request.AccountLegalEntityPublicHashedId, 
                request.Ukprn, request.AccountPublicHashedId, request.StandardLarsCode, request.NationWideOnly, 
                request.Lat, request.Lon, request.DistanceInMiles, categories, request.PostedInLastNumberOfDays, request.Sort));
            var standardsTask = _standardsService.GetStandards();

            await Task.WhenAll(vacanciesTask, standardsTask);

            foreach (var vacanciesItem in vacanciesTask.Result.ApprenticeshipVacancies)
            {
                var standard =
                    standardsTask.Result.Standards.FirstOrDefault(
                        c => c.LarsCode.Equals(vacanciesItem.StandardLarsCode));
                if (standard != null)
                {
                    vacanciesItem.CourseTitle = standard.Title;
                    vacanciesItem.Route = standard.Route;
                    vacanciesItem.CourseLevel = standard.Level;
                }

                vacanciesItem.VacancyUrl = $"{_vacanciesConfiguration.FindAnApprenticeshipBaseUrl}/apprenticeship/reference/{vacanciesItem.VacancyReference}";
            }
            
            return new GetVacanciesQueryResult()
            {
                Vacancies = vacanciesTask.Result.ApprenticeshipVacancies
            };
        }
    }
}
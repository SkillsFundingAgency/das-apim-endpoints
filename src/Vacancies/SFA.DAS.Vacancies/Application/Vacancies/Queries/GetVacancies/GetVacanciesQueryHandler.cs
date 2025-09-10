using System;
using System.Linq;
using System.Security;
using MediatR;
using SFA.DAS.Vacancies.Configuration;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Vacancies.Enums;
using SFA.DAS.Vacancies.InnerApi.Responses;
using SFA.DAS.Vacancies.Services;

namespace SFA.DAS.Vacancies.Application.Vacancies.Queries.GetVacancies
{
    public class GetVacanciesQueryHandler(
        IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient,
        IAccountLegalEntityPermissionService accountLegalEntityPermissionService,
        ICourseService courseService,
        IOptions<VacanciesConfiguration> vacanciesConfiguration,
        IMetrics metrics)
        : IRequestHandler<GetVacanciesQuery, GetVacanciesQueryResult>
    {
        private readonly VacanciesConfiguration _vacanciesConfiguration = vacanciesConfiguration.Value;

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
                        var accountLegalEntity = await accountLegalEntityPermissionService.GetAccountLegalEntity(request.AccountIdentifier,
                                request.AccountLegalEntityPublicHashedId);
                        
                        if (accountLegalEntity == null)
                        {
                            throw new SecurityException();
                        }
                        break;
                    }
                }
            }

            var vacanciesTask = findApprenticeshipApiClient.Get<GetVacanciesResponse>(new GetVacanciesRequest(
                request.PageNumber, request.PageSize, request.AccountLegalEntityPublicHashedId, request.EmployerName,
                request.Ukprn, request.AccountPublicHashedId, request.StandardLarsCode, request.NationWideOnly, 
                request.Lat, request.Lon, request.DistanceInMiles, request.Routes, request.PostedInLastNumberOfDays, request.AdditionalDataSources, request.Sort,
                request.ExcludeNational));
            var standardsTask = courseService.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse));

            await Task.WhenAll(vacanciesTask, standardsTask);
            
            foreach (var vacanciesItem in vacanciesTask.Result.ApprenticeshipVacancies)
            {
                if (vacanciesItem.StandardLarsCode == null)
                {
                    continue;
                }
                
                var standard =
                    standardsTask.Result.Standards.FirstOrDefault(
                        c => c.LarsCode.Equals(vacanciesItem.StandardLarsCode));
                if (standard != null)
                {
                    vacanciesItem.CourseTitle = standard.Title;
                    vacanciesItem.Route = standard.Route;
                    vacanciesItem.CourseLevel = standard.Level;
                }

                vacanciesItem.VacancyUrl = $"{_vacanciesConfiguration.FindAnApprenticeshipBaseUrl}/apprenticeship/reference/{vacanciesItem.VacancyReference.Replace("VAC","")}";

                // increase the count of vacancy appearing in search results counter metrics.
                if(vacanciesItem.VacancySource == DataSource.Raa) metrics.IncreaseVacancySearchResultViews(vacanciesItem.VacancyReference.TrimVacancyReference());
            }
            
            return new GetVacanciesQueryResult
            {
                Vacancies = vacanciesTask.Result.ApprenticeshipVacancies.Where(c=>c.StandardLarsCode!=null).ToList(),
                Total = vacanciesTask.Result.Total,
                TotalFiltered = vacanciesTask.Result.TotalFound,
                TotalPages = request.PageSize != 0 ? (int)Math.Ceiling((decimal)vacanciesTask.Result.TotalFound / request.PageSize) : 0
            };
        }
    }
}
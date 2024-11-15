using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships
{
    public class SearchApprenticeshipsQueryHandler(
        IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient,
        ILocationLookupService locationLookupService,
        ICourseService courseService,
        IMetrics metrics,
        ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
        : IRequestHandler<SearchApprenticeshipsQuery, SearchApprenticeshipsResult>
    {
        public async Task<SearchApprenticeshipsResult> Handle(SearchApprenticeshipsQuery request, CancellationToken cancellationToken)
        {
            var locationTask = locationLookupService.GetLocationInformation(request.Location, default, default, false);
            var routesTask = courseService.GetRoutes();
            var courseLevelsTask = courseService.GetLevels();

            await Task.WhenAll(locationTask, routesTask, courseLevelsTask);

            var location = locationTask.Result;
            var routes = routesTask.Result;
            var courseLevels = courseLevelsTask.Result;

            if (request.SearchTerm != null && Regex.IsMatch(request.SearchTerm, @"^VAC\d{10}$", RegexOptions.None, TimeSpan.FromSeconds(3)))
            {
                var vacancyReferenceResult = await findApprenticeshipApiClient.Get<GetApprenticeshipVacancyItemResponse>(new GetVacancyRequest(request.SearchTerm));

                if (vacancyReferenceResult != null)
                {
                    return new SearchApprenticeshipsResult
                    {
                        TotalApprenticeshipCount = 1,
                        LocationItem = location,
                        Routes = routes.Routes.ToList(),
                        Vacancies = new List<GetVacanciesListItem>
                        {
                            Capacity = 0
                        },
                        PageNumber = request.PageNumber,
                        PageSize = request.PageSize,
                        TotalPages = 1,
                        VacancyReference = request.SearchTerm,
                        Levels = courseLevels.Levels.ToList()
                    };
                }
            }

            var categories = routes.Routes.Where(route => request.SelectedRouteIds != null && request.SelectedRouteIds.Contains(route.Id.ToString()))
                .Select(route => route.Name).ToList();

            var totalWageTypeVacanciesCount = new GetApprenticeshipCountResponse { TotalVacancies = 0 };
            if (request.Sort is VacancySort.SalaryAsc or VacancySort.SalaryDesc)
            {
                totalWageTypeVacanciesCount = await
                    findApprenticeshipApiClient.Get<GetApprenticeshipCountResponse>(
                        new GetApprenticeshipCountRequest(WageType.CompetitiveSalary));
            }

            var vacancyResult = await findApprenticeshipApiClient.Get<GetVacanciesResponse>(
                new GetVacanciesRequest(
                    location?.GeoPoint?.FirstOrDefault(),
                    location?.GeoPoint?.LastOrDefault(),
                    request.Distance,
                    request.SearchTerm,
                    request.PageNumber,
                    request.PageSize,
                    categories,
                    request.SelectedLevelIds,
                    request.Sort,
                    request.SkipWageType,
                    request.DisabilityConfident));

            var totalPages = (int)Math.Ceiling((double)vacancyResult.TotalFound / request.PageSize);

            var apprenticeshipVacancies = new List<GetVacanciesListItem>();

            if (!string.IsNullOrEmpty(request.CandidateId))
            {
                var candidateApplicationsTask =
                    candidateApiClient.Get<GetApplicationsApiResponse>(
                        new GetApplicationsApiRequest(Guid.Parse(request.CandidateId)));

                var savedVacanciesResponseTask =
                    candidateApiClient.Get<GetSavedVacanciesApiResponse>(
                        new GetSavedVacanciesApiRequest(Guid.Parse(request.CandidateId)));

                await Task.WhenAll(candidateApplicationsTask, savedVacanciesResponseTask);

                var candidateApplications = candidateApplicationsTask.Result;
                var savedVacanciesResponse = savedVacanciesResponseTask.Result;

                foreach (var vacancy in vacancyResult.ApprenticeshipVacancies)
                {
                    vacancy.Application = new GetVacanciesListItem.CandidateApplication
                    {
                        Status = candidateApplications.Applications.FirstOrDefault(fil =>
                                fil.VacancyReference.Equals(vacancy.Id,
                                    StringComparison.CurrentCultureIgnoreCase))?
                            .Status
                    };
                    
                    if (savedVacanciesResponse.SavedVacancies.Count > 0 && savedVacanciesResponse.SavedVacancies.Exists(vac => vac.VacancyReference.Equals(vacancy.Id, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        vacancy.IsSavedVacancy = true;
                    }
                    apprenticeshipVacancies.Add(vacancy);
                }
            }
            else
            {
                apprenticeshipVacancies = vacancyResult.ApprenticeshipVacancies.ToList();
            }

            // increase the count of vacancy appearing in search results counter metrics.
            apprenticeshipVacancies.ForEach(vacancy => metrics.IncreaseVacancySearchResultViews(vacancy.Id));

            return new SearchApprenticeshipsResult
            {
                TotalApprenticeshipCount = vacancyResult.Total,
                TotalWageTypeVacanciesCount = totalWageTypeVacanciesCount.TotalVacancies,
                TotalFound = vacancyResult.TotalFound,
                LocationItem = location,
                Routes = routes.Routes.ToList(),
                Vacancies = apprenticeshipVacancies,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalPages = totalPages,
                Levels = courseLevels.Levels.ToList(),
                DisabilityConfident = request.DisabilityConfident
            };
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses.Shared;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships;

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
        var locationTask = locationLookupService.GetLocationInformation(request.Location, 0, 0);
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

        var categories = routes.Routes
            .Where(route => request.SelectedRouteIds != null && request.SelectedRouteIds.Contains(route.Id))
            .Select(route => route.Name).ToList();
            
        var levelIds = courseLevels.Levels
            .Where(level => request.SelectedLevelIds != null && request.SelectedLevelIds.Contains(level.Code))
            .Select(level => level.Code).ToList();

        var totalVacanciesCount = new GetApprenticeshipCountResponse { TotalVacancies = 0 };
        if (request.Sort is VacancySort.SalaryAsc or VacancySort.SalaryDesc)
        {
            totalVacanciesCount = await
                findApprenticeshipApiClient.Get<GetApprenticeshipCountResponse>(
                    new GetApprenticeshipCountRequest(location?.GeoPoint?.FirstOrDefault(),
                        location?.GeoPoint?.LastOrDefault(),
                        request.Distance,
                        request.SearchTerm,
                        request.PageNumber,
                        request.PageSize,
                        categories,
                        levelIds,
                        WageType.CompetitiveSalary,
                        request.DisabilityConfident,
                        new List<VacancyDataSource>
                        {
                            VacancyDataSource.Raa,
                            VacancyDataSource.Nhs
                        },
                        request.ExcludeNational,
                        request.ApprenticeshipTypes));
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
                levelIds,
                request.Sort,
                request.SkipWageType,
                request.DisabilityConfident,
                new List<VacancyDataSource>
                {
                    VacancyDataSource.Nhs
                },
                request.ExcludeNational,
                request.ApprenticeshipTypes));

        var totalPages = (int)Math.Ceiling((double)vacancyResult.TotalFound / request.PageSize);

        var apprenticeshipVacancies = new List<GetVacanciesListItem>();

        var savedSearchesCount = 0;
        var searchAlreadySaved = false;
        DateTime? candidateDateOfBirth = null;

        if (request.CandidateId != null)
        {
            var candidateId = request.CandidateId.Value;
                
            var candidateApplicationsTask =
                candidateApiClient.Get<GetApplicationsApiResponse>(
                    new GetApplicationsApiRequest(candidateId));

            var candidateTask =
                candidateApiClient.Get<GetCandidateApiResponse>(
                    new GetCandidateApiRequest(candidateId.ToString()));

            var savedVacanciesResponseTask =
                candidateApiClient.Get<GetSavedVacanciesApiResponse>(
                    new GetSavedVacanciesApiRequest(candidateId));

            var savedSearchesResponseTask =
                findApprenticeshipApiClient.Get<GetCandidateSavedSearchesApiResponse>(
                    new GetCandidateSavedSearchesApiRequest(candidateId));

            await Task.WhenAll(
                candidateApplicationsTask,
                savedVacanciesResponseTask,
                savedSearchesResponseTask,
                candidateTask
            );

            var candidateApplications = candidateApplicationsTask.Result;
            var savedVacanciesResponse = savedVacanciesResponseTask.Result;
            candidateDateOfBirth = candidateTask.Result.DateOfBirth;

            foreach (var vacancy in vacancyResult.ApprenticeshipVacancies)
            {
                vacancy.Application = new GetVacanciesListItem.CandidateApplication
                {
                    Status = candidateApplications.Applications.FirstOrDefault(fil =>
                            fil.VacancyReference.Equals(vacancy.Id,
                                StringComparison.CurrentCultureIgnoreCase))?
                        .Status
                };
                    
                if (savedVacanciesResponse.SavedVacancies.Count > 0 && savedVacanciesResponse.SavedVacancies.Exists(vac => vac.VacancyId?.Equals(vacancy.Id, StringComparison.CurrentCultureIgnoreCase) == true ||
                        (vac.VacancyReference.Equals(vacancy.Id, StringComparison.CurrentCultureIgnoreCase) && vac.VacancyId == null)))
                {
                    vacancy.IsSavedVacancy = true;
                }
                apprenticeshipVacancies.Add(vacancy);
            }

            var savedSearchesResponse = savedSearchesResponseTask.Result;
            var searchParameters = new SearchParametersDto(
                request.SearchTerm,
                request.SelectedRouteIds?.Select(x => Convert.ToInt32(x)).ToList(),
                request.Distance,
                request.DisabilityConfident,
                request.ExcludeNational,
                levelIds,
                request.Location,
                location?.GeoPoint?.FirstOrDefault().ToString(CultureInfo.InvariantCulture),
                location?.GeoPoint?.LastOrDefault().ToString(CultureInfo.InvariantCulture)
            );
                
            savedSearchesCount = savedSearchesResponse.SavedSearches?.Count ?? 0;
            searchAlreadySaved = savedSearchesResponse.SavedSearches?.Any(x => x.SearchParameters.Equals(searchParameters)) ?? false;
        } 
        else
        {
            apprenticeshipVacancies = vacancyResult.ApprenticeshipVacancies.ToList();
        }

        // increase the count of vacancy appearing in search results counter metrics.
        foreach (var vacancy in apprenticeshipVacancies.Where(fil => fil.VacancySource == VacancyDataSource.Raa))
        {
            metrics.IncreaseVacancySearchResultViews(vacancy.VacancyReference.TrimVacancyReference());
        }

        return new SearchApprenticeshipsResult
        {
            TotalApprenticeshipCount = vacancyResult.Total,
            TotalWageTypeVacanciesCount = totalVacanciesCount.TotalVacancies,
            TotalFound = vacancyResult.TotalFound,
            LocationItem = location,
            Routes = routes.Routes.ToList(),
            Vacancies = apprenticeshipVacancies,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalPages = totalPages,
            Levels = courseLevels.Levels.ToList(),
            DisabilityConfident = request.DisabilityConfident,
            SavedSearchesCount = savedSearchesCount,
            SearchAlreadySaved = searchAlreadySaved,
            CandidateDateOfBirth = candidateDateOfBirth
        };
    }
}
using System.Collections.Concurrent;
using MediatR;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetSavedSearches
{
    public record GetSavedSearchesQueryHandler(
        IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> FindApprenticeshipApiClient,
        ICourseService CourseService,
        ICandidateApiClient<CandidateApiConfiguration> CandidateApiClient)
        : IRequestHandler<GetSavedSearchesQuery, GetSavedSearchesQueryResult>
    {
        public async Task<GetSavedSearchesQueryResult> Handle(GetSavedSearchesQuery request,
            CancellationToken cancellationToken)
        {
            var searchResultList = new ConcurrentBag<GetSavedSearchesQueryResult.SearchResult>();

            var savedSearchResponse = await FindApprenticeshipApiClient.Get<GetSavedSearchesApiResponse>(
                new GetSavedSearchesApiRequest(request.LastRunDateFilter.ToString("O"),
                    request.PageNumber,
                    request.PageSize));

            if (savedSearchResponse is not {SavedSearches.Count: > 0})
                return new GetSavedSearchesQueryResult
                {
                    PageSize = savedSearchResponse.PageSize,
                    PageIndex = savedSearchResponse.PageIndex,
                    TotalPages = savedSearchResponse.TotalPages,
                    TotalCount = savedSearchResponse.TotalCount,
                    SavedSearchResults = []
                };

            foreach (var savedSearch in savedSearchResponse.SavedSearches)
            {
                var candidate =
                    await CandidateApiClient.Get<GetCandidateApiResponse>(
                        new GetCandidateApiRequest(savedSearch.UserReference.ToString()));

                if (candidate == null || candidate.Status == UserStatus.Deleted ||
                    candidate.Status == UserStatus.Dormant) continue;

                var routesTask = CourseService.GetRoutes();
                var levelsTask = CourseService.GetLevels();

                await Task.WhenAll(routesTask, levelsTask);
                var routesList = routesTask.Result;
                var levelsList = levelsTask.Result;
                var taskList = savedSearchResponse.SavedSearches.Select(savedSearch =>
                    GetSavedSearchResults(request, savedSearch, routesList, levelsList, searchResultList)).ToList();
                await Task.WhenAll(taskList);
                return new GetSavedSearchesQueryResult
                {
                    PageSize = savedSearchResponse.PageSize,
                    PageIndex = savedSearchResponse.PageIndex,
                    TotalPages = savedSearchResponse.TotalPages,
                    TotalCount = savedSearchResponse.TotalCount,
                    SavedSearchResults = searchResultList.ToList()
                };
            }

            return new GetSavedSearchesQueryResult
            {
                PageSize = savedSearchResponse.PageSize,
                PageIndex = savedSearchResponse.PageIndex,
                TotalPages = savedSearchResponse.TotalPages,
                TotalCount = savedSearchResponse.TotalCount,
                SavedSearchResults = searchResultList.ToList()
            };
        }

        private async Task GetSavedSearchResults(GetSavedSearchesQuery request,
            GetSavedSearchesApiResponse.SavedSearch savedSearch,
            GetRoutesListResponse routesList, GetCourseLevelsListResponse levelsList,
            ConcurrentBag<GetSavedSearchesQueryResult.SearchResult> searchResultList)
        {
            var candidate =
                await CandidateApiClient.Get<GetCandidateApiResponse>(
                    new GetCandidateApiRequest(savedSearch.UserReference.ToString()));

            if (candidate == null || candidate.Status == UserStatus.Deleted) return;

            var categories = routesList.Routes
                .Where(route =>
                    savedSearch.SearchParameters.SelectedRouteIds != null
                    && savedSearch.SearchParameters.SelectedRouteIds.Contains(route.Id))
                .Select(route => new GetSavedSearchesQueryResult.SearchResult.Category
                {
                    Id = route.Id,
                    Name = route.Name,
                }).ToList();

            var levels = levelsList.Levels
                .Where(level =>
                    savedSearch.SearchParameters.SelectedLevelIds != null &&
                    savedSearch.SearchParameters.SelectedLevelIds.Contains(level.Code))
                .Select(level => new GetSavedSearchesQueryResult.SearchResult.Level
                {
                    Code = level.Code,
                    Name = level.Name,
                }).ToList();

            var vacanciesResponse = await FindApprenticeshipApiClient.Get<GetVacanciesResponse>(
                new GetVacanciesRequest(
                    !string.IsNullOrEmpty(savedSearch.SearchParameters.Latitude)
                        ? Convert.ToDouble(savedSearch.SearchParameters.Latitude)
                        : null,
                    !string.IsNullOrEmpty(savedSearch.SearchParameters.Longitude)
                        ? Convert.ToDouble(savedSearch.SearchParameters.Longitude)
                        : null,
                    savedSearch.SearchParameters.Distance,
                    savedSearch.SearchParameters.SearchTerm,
                    1, // Defaulting to top results.
                    request.MaxApprenticeshipSearchResultsCount, // Default page size set to 5.
                    categories.Select(cat => cat.Id.ToString()).ToList(),
                    savedSearch.SearchParameters.SelectedLevelIds,
                    request.ApprenticeshipSearchResultsSortOrder,
                    savedSearch.SearchParameters.DisabilityConfident,
                    new List<VacancyDataSource>
                    {
                        VacancyDataSource.Nhs
                    }));

            if (vacanciesResponse != null)
            {
                var searchResult = new GetSavedSearchesQueryResult.SearchResult
                {
                    Id = savedSearch.Id,
                    User = candidate,
                    SearchTerm = savedSearch.SearchParameters.SearchTerm,
                    Location = savedSearch.SearchParameters.Location,
                    Categories = categories,
                    Levels = levels,
                    DisabilityConfident = savedSearch.SearchParameters.DisabilityConfident,
                    Distance = savedSearch.SearchParameters.Distance,
                    UnSubscribeToken = savedSearch.UnSubscribeToken,
                    Vacancies = vacanciesResponse.ApprenticeshipVacancies.Select(x =>
                            (GetSavedSearchesQueryResult.SearchResult.ApprenticeshipVacancy) x)
                        .ToList()
                };
                searchResultList.Add(searchResult);
            }
        }

    }
}
using MediatR;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetSavedSearches
{
    public record GetSavedSearchesQueryHandler(
        IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> FindApprenticeshipApiClient,
        ICourseService CourseService,
        ICandidateApiClient<CandidateApiConfiguration> CandidateApiClient)
        : IRequestHandler<GetSavedSearchesQuery, GetSavedSearchesQueryResult>
    {
        public async Task<GetSavedSearchesQueryResult> Handle(GetSavedSearchesQuery request, CancellationToken cancellationToken)
        {
            var searchResultList = new List<GetSavedSearchesQueryResult.SearchResult>();

            var savedSearchResponse = await FindApprenticeshipApiClient.Get<GetSavedSearchesApiResponse>(
                new GetSavedSearchesApiRequest(request.LastRunDateFilter.ToString("O"),
                    request.PageNumber,
                    request.PageSize));

            if (savedSearchResponse is not { SavedSearches.Count: > 0 })
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

                if (candidate == null || candidate.Status == UserStatus.Deleted) continue;

                var routesTask = CourseService.GetRoutes();
                var levelsTask = CourseService.GetLevels();

                await Task.WhenAll(routesTask, levelsTask);

                var routesList = routesTask.Result;
                var levelsList = levelsTask.Result;

                var categories = routesList.Routes
                    .Where(route =>
                        savedSearch.SearchCriteriaParameters.Categories != null
                        && savedSearch.SearchCriteriaParameters.Categories.Contains(route.Id.ToString()))
                    .Select(route => route.Name).ToList();

                var levels = levelsList.Levels
                    .Where(level =>
                        savedSearch.SearchCriteriaParameters.Levels != null &&
                        savedSearch.SearchCriteriaParameters.Levels.Contains(level.Code.ToString()))
                    .Select(level => level.Name).ToList();

                var vacanciesResponse = await FindApprenticeshipApiClient.Get<GetVacanciesResponse>(
                    new GetVacanciesRequest(
                        !string.IsNullOrEmpty(savedSearch.SearchCriteriaParameters.Latitude) ? Convert.ToDouble(savedSearch.SearchCriteriaParameters.Latitude) : null,
                        !string.IsNullOrEmpty(savedSearch.SearchCriteriaParameters.Longitude) ? Convert.ToDouble(savedSearch.SearchCriteriaParameters.Longitude) : null,
                        savedSearch.SearchCriteriaParameters.Distance,
                        savedSearch.SearchCriteriaParameters.SearchTerm,
                        1,  // Defaulting to top results.
                        request.MaxApprenticeshipSearchResultsCount, // Default page size set to 10.
                        categories,
                        savedSearch.SearchCriteriaParameters.Levels,
                        request.ApprenticeshipSearchResultsSortOrder,
                        savedSearch.SearchCriteriaParameters.DisabilityConfident));

                searchResultList.Add(new GetSavedSearchesQueryResult.SearchResult
                {
                    Id = savedSearch.Id,
                    User = candidate,
                    SearchTerm = savedSearch.SearchCriteriaParameters.SearchTerm,
                    Location = savedSearch.SearchCriteriaParameters.Location,
                    Categories = categories,
                    Levels = levels,
                    DisabilityConfident = savedSearch.SearchCriteriaParameters.DisabilityConfident,
                    Distance = savedSearch.SearchCriteriaParameters.Distance,
                    Vacancies = vacanciesResponse.ApprenticeshipVacancies.Select(x => (GetSavedSearchesQueryResult.SearchResult.ApprenticeshipVacancy)x)
                        .ToList()
                });
            }

            return new GetSavedSearchesQueryResult
            {
                PageSize = savedSearchResponse.PageSize,
                PageIndex = savedSearchResponse.PageIndex,
                TotalPages = savedSearchResponse.TotalPages,
                TotalCount = savedSearchResponse.TotalCount,
                SavedSearchResults = searchResultList
            };
        }
    }
}

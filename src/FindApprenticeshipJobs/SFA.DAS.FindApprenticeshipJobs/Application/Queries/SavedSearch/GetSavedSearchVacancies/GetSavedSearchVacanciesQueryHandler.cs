using MediatR;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetSavedSearchVacancies;

public class GetSavedSearchVacanciesQueryHandler(
    IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient,
    ICourseService courseService,
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient) : IRequestHandler<GetSavedSearchVacanciesQuery, GetSavedSearchVacanciesQueryResult?>
{
    public async Task<GetSavedSearchVacanciesQueryResult?> Handle(GetSavedSearchVacanciesQuery request, CancellationToken cancellationToken)
    {
        var candidate =
            await candidateApiClient.Get<GetCandidateApiResponse>(
                new GetCandidateApiRequest(request.UserId.ToString()));

        if (candidate == null || candidate.Status == UserStatus.Deleted || candidate.Status == UserStatus.Dormant)
        {
            return null;
        }

        var routesTask = courseService.GetRoutes();
        var levelsTask = courseService.GetLevels();

        await Task.WhenAll(routesTask, levelsTask);
        var routesList = routesTask.Result;
        var levelsList = levelsTask.Result;
        
        return await GetSavedSearchResults(request, routesList, levelsList, candidate);
    }
    private async Task<GetSavedSearchVacanciesQueryResult?> GetSavedSearchResults(
        GetSavedSearchVacanciesQuery request,
        GetRoutesListResponse routesList,
        GetCourseLevelsListResponse levelsList,
        GetCandidateApiResponse candidate)
    {
        var categories = routesList.Routes
            .Where(route => request.SelectedRouteIds?.Contains(route.Id) ?? false)
            .Select(route => new GetSavedSearchVacanciesQueryResult.Category
            {
                Id = route.Id,
                Name = route.Name,
            }).ToList();

        var levels = levelsList.Levels
            .Where(level => request.SelectedLevelIds?.Contains(level.Code) ?? false)
            .Select(level => new GetSavedSearchVacanciesQueryResult.Level
            {
                Code = level.Code,
                Name = level.Name,
            }).ToList();

        var vacanciesResponse = await findApprenticeshipApiClient.Get<GetVacanciesResponse>(
            new GetVacanciesRequest(
                !string.IsNullOrEmpty(request.Latitude)
                    ? Convert.ToDouble(request.Latitude)
                    : null,
                !string.IsNullOrEmpty(request.Longitude)
                    ? Convert.ToDouble(request.Longitude)
                    : null,
                request.Distance,
                request.SearchTerm,
                1, // Defaulting to top results.
                5, // Default page size set to 5.
                categories.Select(cat => cat.Name!).ToList(),
                levels.Select(level => level.Code).ToList(),
                request.ApprenticeshipSearchResultsSortOrder,
                request.DisabilityConfident,
                request.ExcludeNational,
                [
                    VacancyDataSource.Nhs
                ],
                request.SelectedApprenticeshipTypes)
        );

        var searchResult = new GetSavedSearchVacanciesQueryResult
        {
            Id = request.Id,
            User = candidate,
            SearchTerm = request.SearchTerm,
            Location = request.Location,
            Categories = categories,
            Levels = levels,
            DisabilityConfident = request.DisabilityConfident,
            ExcludeNational = request.ExcludeNational,
            Distance = request.Distance,
            UnSubscribeToken = request.UnSubscribeToken,
            Vacancies = vacanciesResponse?.ApprenticeshipVacancies != null ? vacanciesResponse.ApprenticeshipVacancies.Select(x =>
                    (GetSavedSearchVacanciesQueryResult.ApprenticeshipVacancy) x)
                .ToList() : []
        };
        return searchResult;
        
    }
}
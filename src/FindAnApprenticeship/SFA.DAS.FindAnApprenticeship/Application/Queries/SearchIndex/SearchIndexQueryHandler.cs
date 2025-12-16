using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses.Shared;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchIndex;

public class SearchIndexQueryHandler(
    ITotalPositionsAvailableService totalPositionsAvailableService,
    ILocationLookupService locationLookupService,
    ICourseService courseService,
    IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient)
    : IRequestHandler<SearchIndexQuery, SearchIndexQueryResult>
{
    public async Task<SearchIndexQueryResult> Handle(SearchIndexQuery request, CancellationToken cancellationToken)
    {
        var locationTask = locationLookupService.GetLocationInformation(request.LocationSearchTerm, 0, 0, false);
        var totalPositionsCountTask = totalPositionsAvailableService.GetTotalPositionsAvailable();

        List<GetRoutesListItem> routes = null;
        List<SavedSearchDto> savedSearches = null;
        var routesTask = Task.FromResult<GetRoutesListResponse>(null);
        var savedSearchesTask = Task.FromResult<GetCandidateSavedSearchesApiResponse>(null);

        if (request.CandidateId.HasValue)
        {
            routesTask = courseService.GetRoutes();
            savedSearchesTask = findApprenticeshipApiClient.Get<GetCandidateSavedSearchesApiResponse>(new GetCandidateSavedSearchesApiRequest(request.CandidateId.Value));
        }
        var tasks = new List<Task> { locationTask, totalPositionsCountTask, routesTask, savedSearchesTask };

        await Task.WhenAll(tasks);

        var location = locationTask.Result;
        savedSearches = savedSearchesTask.Result?.SavedSearches.ToList();
        routes = routesTask.Result?.Routes.ToList();

        return new SearchIndexQueryResult
        {
            TotalApprenticeshipCount = totalPositionsCountTask.Result,
            LocationSearched = !string.IsNullOrEmpty(request.LocationSearchTerm),
            LocationItem = location,
            SavedSearches = savedSearches?.Select(c => c.ToDomain()).ToList(),
            Routes = routes
        };
    }
}
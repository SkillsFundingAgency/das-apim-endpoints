using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchIndex;

public class SearchIndexQueryHandler : IRequestHandler<SearchIndexQuery, SearchIndexQueryResult>
{
    private readonly IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> _findApprenticeshipApiClient;
    private readonly ILocationLookupService _locationLookupService;

    public SearchIndexQueryHandler(IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient, ILocationLookupService locationLookupService)
    {
        _findApprenticeshipApiClient = findApprenticeshipApiClient;
        _locationLookupService = locationLookupService;
    }
    
    public async Task<SearchIndexQueryResult> Handle(SearchIndexQuery request, CancellationToken cancellationToken)
    {
        var resultTask = _findApprenticeshipApiClient.Get<GetApprenticeshipCountResponse>(
            new GetApprenticeshipCountRequest());

        var locationTask = _locationLookupService.GetLocationInformation(request.LocationSearchTerm, 0, 0, false);

        await Task.WhenAll(resultTask, locationTask);

        var result = resultTask.Result;
        var location = locationTask.Result;
        
        return new SearchIndexQueryResult
        {
            TotalApprenticeshipCount = result.TotalVacancies,
            LocationSearched = !string.IsNullOrEmpty(request.LocationSearchTerm),
            LocationItem = location
        };
    }
}
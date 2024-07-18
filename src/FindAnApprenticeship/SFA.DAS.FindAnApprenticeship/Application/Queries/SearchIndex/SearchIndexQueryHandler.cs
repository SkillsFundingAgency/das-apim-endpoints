using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchIndex;

public class SearchIndexQueryHandler(
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
    ILocationLookupService locationLookupService)
    : IRequestHandler<SearchIndexQuery, SearchIndexQueryResult>
{
    public async Task<SearchIndexQueryResult> Handle(SearchIndexQuery request, CancellationToken cancellationToken)
    {
        var locationTask = locationLookupService.GetLocationInformation(request.LocationSearchTerm, 0, 0, false);

        var totalPositionsCountTask = recruitApiClient.Get<long>(new GetTotalPositionsAvailableRequest());

        await Task.WhenAll(locationTask, totalPositionsCountTask);

        var location = locationTask.Result;
        
        return new SearchIndexQueryResult
        {
            TotalApprenticeshipCount = totalPositionsCountTask.Result,
            LocationSearched = !string.IsNullOrEmpty(request.LocationSearchTerm),
            LocationItem = location
        };
    }
}
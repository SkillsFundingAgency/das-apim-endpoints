using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchIndex;

public class SearchIndexQueryHandler : IRequestHandler<SearchIndexQuery, SearchIndexQueryResult>
{
    private readonly IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> _findApprenticeshipApiClient;

    public SearchIndexQueryHandler(IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient)
    {
        _findApprenticeshipApiClient = findApprenticeshipApiClient;
    }
    
    public async Task<SearchIndexQueryResult> Handle(SearchIndexQuery request, CancellationToken cancellationToken)
    {
        var result = await _findApprenticeshipApiClient.Get<GetApprenticeshipCountResponse>(
            new GetApprenticeshipCountRequest(
                null,
                null,
                null,
                null
            ));

        return new SearchIndexQueryResult
        {
            TotalApprenticeshipCount = result.TotalVacancies
        };
    }
}
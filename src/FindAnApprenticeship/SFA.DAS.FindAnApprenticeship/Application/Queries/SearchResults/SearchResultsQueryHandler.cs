using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchResults
{
    public class SearchResultsQueryHandler : IRequestHandler<SearchResultsQuery, SearchResultsQueryResult>
    {
        private readonly IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> _findApprenticeshipApiClient;

        public SearchResultsQueryHandler(IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient)
        {
            _findApprenticeshipApiClient = findApprenticeshipApiClient;
        }

        public async Task<SearchResultsQueryResult> Handle(SearchResultsQuery request, CancellationToken cancellationToken)
        {
            var result = await _findApprenticeshipApiClient.Get<GetApprenticeshipCountResponse>(
                    new GetApprenticeshipCountRequest());

            return new SearchResultsQueryResult
            {
                TotalApprenticeshipCount = result.TotalVacancies
            };
        }
    }
}

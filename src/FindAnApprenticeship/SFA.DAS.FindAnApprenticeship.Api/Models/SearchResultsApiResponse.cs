using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchResults;

namespace SFA.DAS.FindAnApprenticeship.Api.Models
{
    public class SearchResultsApiResponse
    {
        public long TotalApprenticeshipCount { get; set; }

        public static implicit operator SearchResultsApiResponse(SearchResultsQueryResult source)
        {
            return new SearchResultsApiResponse
            {
                TotalApprenticeshipCount = source.TotalApprenticeshipCount
            };
        }
    }
}

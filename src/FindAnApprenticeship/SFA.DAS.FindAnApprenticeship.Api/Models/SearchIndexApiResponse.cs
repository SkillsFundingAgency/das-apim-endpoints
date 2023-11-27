using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchIndex;

namespace SFA.DAS.FindAnApprenticeship.Api.Models;

public class SearchIndexApiResponse
{
    public static implicit operator SearchIndexApiResponse(SearchIndexQueryResult source)
    {
        return new SearchIndexApiResponse
        {
            TotalApprenticeshipCount = source.TotalApprenticeshipCount
        };
    }

    public long TotalApprenticeshipCount { get; set; }
}
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchIndex;

public class SearchIndexQueryResult
{
    public long TotalApprenticeshipCount { get; set; }
    public LocationItem LocationItem { get; set; }
    public bool LocationSearched { get; set; }
}
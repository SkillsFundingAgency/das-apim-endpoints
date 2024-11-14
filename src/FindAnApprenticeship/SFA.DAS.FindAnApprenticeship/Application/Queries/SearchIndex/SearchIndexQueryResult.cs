using System.Collections.Generic;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchIndex;

public class SearchIndexQueryResult
{
    public long TotalApprenticeshipCount { get; set; }
    public LocationItem LocationItem { get; set; }
    public bool LocationSearched { get; set; }
    
    public List<GetRoutesListItem> Routes { get; set; }
    
    public List<SavedSearch> SavedSearches { get; set; }
}
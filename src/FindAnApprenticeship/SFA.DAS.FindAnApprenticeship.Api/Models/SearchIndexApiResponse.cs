using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchIndex;
using SFA.DAS.FindAnApprenticeship.Domain.Models;

namespace SFA.DAS.FindAnApprenticeship.Api.Models;

public class SearchIndexApiResponse
{
    public static implicit operator SearchIndexApiResponse(SearchIndexQueryResult source)
    {
        return new SearchIndexApiResponse
        {
            TotalApprenticeshipCount = source.TotalApprenticeshipCount,
            Location = source.LocationItem,
            LocationSearched = source.LocationSearched,
            SavedSearches = source.SavedSearches,
            Routes = source.Routes?.Select(c=>(RouteApiResponse)c).ToList()
        };
    }

    public List<SavedSearch> SavedSearches { get; set; }

    [JsonPropertyName("location")]
    public SearchLocationApiResponse Location { get; set; }
    [JsonPropertyName("totalApprenticeshipCount")]
    public long TotalApprenticeshipCount { get; set; }
    [JsonPropertyName("locationSearched")]
    public bool LocationSearched { get; set; }
    
    public List<RouteApiResponse> Routes { get; set; }
}
using System.Text.Json.Serialization;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchIndex;

namespace SFA.DAS.FindAnApprenticeship.Api.Models;

public class SearchIndexApiResponse
{
    public static implicit operator SearchIndexApiResponse(SearchIndexQueryResult source)
    {
        return new SearchIndexApiResponse
        {
            TotalApprenticeshipCount = source.TotalApprenticeshipCount,
            Location = source.LocationItem,
            LocationSearched = source.LocationSearched
        };
    }
    
    [JsonPropertyName("location")]
    public SearchLocationApiResponse Location { get; set; }
    [JsonPropertyName("totalApprenticeshipCount")]
    public long TotalApprenticeshipCount { get; set; }
    [JsonPropertyName("locationSearched")]
    public bool LocationSearched { get; set; }
}
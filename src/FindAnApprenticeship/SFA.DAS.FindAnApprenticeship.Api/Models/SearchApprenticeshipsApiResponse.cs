using System.Text.Json.Serialization;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships;

namespace SFA.DAS.FindAnApprenticeship.Api.Models
{
    public class SearchApprenticeshipsApiResponse
    {
        public static implicit operator SearchApprenticeshipsApiResponse(SearchApprenticeshipsResult source)
        {
            return new SearchApprenticeshipsApiResponse
            {
                TotalApprenticeshipCount = source.TotalApprenticeshipCount
            };
        }
        
        [JsonPropertyName("totalApprenticeshipCount")]
        public long TotalApprenticeshipCount { get; set; }
    }
}
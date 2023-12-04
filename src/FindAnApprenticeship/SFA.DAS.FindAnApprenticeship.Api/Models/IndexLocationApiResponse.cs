using System.Text.Json.Serialization;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchIndexLocation;

namespace SFA.DAS.FindAnApprenticeship.Api.Models
{
    public class IndexLocationApiResponse
    {
        [JsonPropertyName("location")]
        public SearchLocationApiResponse Location { get; set; }

        public static implicit operator IndexLocationApiResponse(IndexLocationQueryResult source)
        {
            if (source.LocationItem == null)
            {
                return new IndexLocationApiResponse
                {
                    Location = null
                };
            }
            return new IndexLocationApiResponse
            {
                Location = source.LocationItem
            };
        }
    }
}
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using SFA.DAS.FindAnApprenticeship.Application.Queries.BrowseByInterestsLocation;

namespace SFA.DAS.FindAnApprenticeship.Api.Models
{
    public class BrowseByInterestsLocationApiResponse
    {
        [JsonPropertyName("location")]
        public SearchLocationApiResponse Location { get; set; }

        public static implicit operator BrowseByInterestsLocationApiResponse(BrowseByInterestsLocationQueryResult source)
        {
            if (source.LocationItem == null)
            {
                return new BrowseByInterestsLocationApiResponse
                {
                    Location = null
                };
            }
            return new BrowseByInterestsLocationApiResponse
            {
                Location = source.LocationItem
            };
        }
    }
}
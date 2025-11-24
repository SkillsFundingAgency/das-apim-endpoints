using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.Models
{
    public record Address
    {
        [JsonPropertyName("addressLine1")]
        public string AddressLine1 { get; set; }
        [JsonPropertyName("addressLine2")]
        public string AddressLine2 { get; set; }
        [JsonPropertyName("addressLine3")]
        public string AddressLine3 { get; set; }
        [JsonPropertyName("addressLine4")]
        public string AddressLine4 { get; set; }
        [JsonPropertyName("postcode")]
        public string Postcode { get; set; }

        public double? Latitude { get; set; } = 0.00;
        public double? Longitude { get; set; } = 0.00;
        public string? Country { get; set; }
    }
}
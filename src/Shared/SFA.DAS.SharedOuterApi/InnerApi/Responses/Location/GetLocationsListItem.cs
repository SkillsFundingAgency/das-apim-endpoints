using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses
{
    public class GetLocationsListItem
    {
        [JsonPropertyName("localAuthorityName")]
        public string LocalAuthorityName { get; set; }

        [JsonPropertyName("countyName")]
        public string CountyName { get; set; }

        [JsonPropertyName("locationName")]
        public string LocationName { get; set; }
        
        [JsonPropertyName("location")]
        public Coordinates Location { get; set; }
        [JsonPropertyName("postcode")]
        public string Postcode { get; set; }
        [JsonPropertyName("districtName")]
        public string DistrictName { get; set; }
        [JsonPropertyName("outcode")]
        public string Outcode { get; set; }
        [JsonPropertyName("country")]
        public string Country { get; set; }

        public class Coordinates
        {
            [JsonPropertyName("coordinates")]
            public double[] GeoPoint { get; set; }
        }

        [JsonIgnore] 
        public string DisplayName => GetDisplayName();

        [JsonIgnore]
        public bool IncludeDistrictNameInPostcodeDisplayName { get ; set ; }

        private string GetDisplayName()
        {
            return (!string.IsNullOrEmpty(Outcode) && !string.IsNullOrEmpty(DistrictName)) ? $"{Outcode} {DistrictName}" :
                string.IsNullOrEmpty(Postcode) ? $"{LocationName}, {LocalAuthorityName}" : GetPostcodeDisplayName();
        }

        private string GetPostcodeDisplayName()
        {
            if (IncludeDistrictNameInPostcodeDisplayName)
            {
                return $"{Postcode}, {DistrictName}";
            }
            return $"{Postcode}";
        }
    }
}

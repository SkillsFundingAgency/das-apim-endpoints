using Newtonsoft.Json;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses
{
    public class GetLocationsListItem
    {
        [JsonProperty("localAuthorityName")]
        public string LocalAuthorityName { get; set; }

        [JsonProperty("countyName")]
        public string CountyName { get; set; }

        [JsonProperty("locationName")]
        public string LocationName { get; set; }
        
        [JsonProperty("location")]
        public Coordinates Location { get; set; }
        [JsonProperty("postcode")]
        public string Postcode { get; set; }
        [JsonProperty("districtName")]
        public string DistrictName { get; set; }
        [JsonProperty("outcode")]
        public string Outcode { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }

        public class Coordinates
        {
            [JsonProperty("coordinates")]
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

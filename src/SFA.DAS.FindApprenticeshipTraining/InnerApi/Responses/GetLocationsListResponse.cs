using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetLocationsListResponse
    {
        public IEnumerable<GetLocationsListItem> Locations { get; set; }
    }

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

        public class Coordinates
        {
            [JsonProperty("coordinates")]
            public double[] GeoPoint { get; set; }
        }
    }
}
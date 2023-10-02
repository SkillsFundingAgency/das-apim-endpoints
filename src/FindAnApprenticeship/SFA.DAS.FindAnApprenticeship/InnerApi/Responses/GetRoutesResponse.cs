using Newtonsoft.Json;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.Responses
{
    public class GetRoutesResponse
    {
        public List<RouteResponse> Routes { get; set; }
    }

    public class RouteResponse
    {
        [JsonProperty("route")]
        public string Route { get; set; }
    }
}
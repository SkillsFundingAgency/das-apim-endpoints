using Newtonsoft.Json;

namespace SFA.DAS.Vacancies.InnerApi.Responses
{
    public class Location
    {
        [JsonProperty("lon")]
        public double Lon { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }
    }
}
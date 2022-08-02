using System.Text.Json.Serialization;

namespace SFA.DAS.Vacancies.InnerApi.Responses
{
    public class Location
    {
        [JsonPropertyName("lon")]
        public double Lon { get; set; }

        [JsonPropertyName("lat")]
        public double Lat { get; set; }
    }
}
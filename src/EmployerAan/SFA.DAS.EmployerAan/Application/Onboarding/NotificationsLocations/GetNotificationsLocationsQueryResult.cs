using System.Text.Json.Serialization;

namespace SFA.DAS.EmployerAan.Application.Onboarding.NotificationsLocations;

public class GetNotificationsLocationsQueryResult
{
    public List<Location> Locations { get; set; } = [];

    public class Location
    {
        public string Name { get; set; }
        [JsonPropertyName("coordinates")]
        public double[] GeoPoint { get; set; }
    }
}
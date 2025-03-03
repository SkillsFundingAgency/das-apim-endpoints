using System.Text.Json.Serialization;

namespace SFA.DAS.EmployerAan.Application.Settings.NotificationsLocations;

public class GetNotificationsLocationsQueryResult
{
    public List<Location> Locations { get; set; } = [];

    public List<AddedLocation> SavedLocations { get; set; } = [];
    public List<NotificationEventType> NotificationEventTypes { get; set; } = [];

    public class Location
    {
        public string Name { get; set; }
        [JsonPropertyName("coordinates")]
        public double[] GeoPoint { get; set; }
    }

    public class NotificationEventType
    {
        public string EventFormat { get; set; }
        public bool ReceiveNotifications { get; set; }
    }

    public class AddedLocation
    {
        public string Name { get; set; }
        public int Radius { get; set; }
        public double[] Coordinates { get; set; }
    }
}
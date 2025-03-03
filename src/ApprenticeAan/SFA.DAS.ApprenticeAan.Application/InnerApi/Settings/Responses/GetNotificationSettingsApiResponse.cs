namespace SFA.DAS.ApprenticeAan.Application.InnerApi.Settings.Responses
{
    public class GetNotificationSettingsApiResponse
    {
        public IEnumerable<NotificationEventType> EventTypes { get; set; } = Enumerable.Empty<NotificationEventType>();
        public IEnumerable<Location> Locations { get; set; } = Enumerable.Empty<Location>();
    }

    public class NotificationEventType
    {
        public string EventType { get; set; } = string.Empty;
        public bool ReceiveNotifications { get; set; }
    }

    public class Location
    {
        public string Name { get; set; } = string.Empty;
        public int Radius { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}

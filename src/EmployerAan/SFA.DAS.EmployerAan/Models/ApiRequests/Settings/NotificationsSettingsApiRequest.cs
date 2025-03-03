namespace SFA.DAS.EmployerAan.Models.ApiRequests.Settings
{
    public class NotificationsSettingsApiRequest
    {
        public bool ReceiveNotifications { get; set; }
        public List<NotificationEventType> EventTypes { get; set; } = [];
        public List<Location> Locations { get; set; } = [];

        public class NotificationEventType
        {
            public string EventType { get; set; }
            public bool ReceiveNotifications { get; set; }
        }

        public class Location
        {
            public string Name { get; set; }
            public int Radius { get; set; }
            public double Latitude { get; set;}
            public double Longitude { get; set; }
        }
    }
}

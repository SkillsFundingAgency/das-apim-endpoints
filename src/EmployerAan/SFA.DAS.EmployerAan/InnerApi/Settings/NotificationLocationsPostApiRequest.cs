namespace SFA.DAS.EmployerAan.InnerApi.Settings
{
    public class NotificationLocationsPostApiRequest
    {
        public bool ReceiveNotifications { get; set; }
        public List<NotificationEventType> EventTypes = [];
        public List<Location> Locations { get; set; } = [];

        public class NotificationEventType
        {
            public string EventType { get; set; }
            public int Ordering { get; set; }
            public bool ReceiveNotifications { get; set; }
        }

        public class Location
        {
            public string Name { get; set; }
            public int Radius { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
    }
}

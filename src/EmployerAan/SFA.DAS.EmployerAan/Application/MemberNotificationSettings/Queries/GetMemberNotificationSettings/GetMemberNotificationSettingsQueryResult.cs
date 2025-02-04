namespace SFA.DAS.EmployerAan.Application.MemberNotificationSettings.Queries.GetMemberNotificationSettings;

public class GetMemberNotificationSettingsQueryResult
{
    public IEnumerable<EventType> MemberNotificationEventFormats { get; set; } = [];
    public IEnumerable<Location> MemberNotificationLocations { get; set; } = [];
    public bool? ReceiveMonthlyNotifications { get; set; }

    public class EventType
    {
        public string EventFormat { get; set; } = string.Empty;
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

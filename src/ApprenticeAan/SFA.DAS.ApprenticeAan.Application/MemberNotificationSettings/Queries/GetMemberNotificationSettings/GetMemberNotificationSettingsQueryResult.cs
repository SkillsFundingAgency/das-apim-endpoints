namespace SFA.DAS.ApprenticeAan.Application.MemberNotificationSettings.Queries.GetMemberNotificationSettings;

public class GetMemberNotificationSettingsQueryResult
{
    public IEnumerable<MemberNotificationEventFormatModel> MemberNotificationEventFormats { get; set; } = Enumerable.Empty<MemberNotificationEventFormatModel>();
    public IEnumerable<MemberNotificationLocationsModel> MemberNotificationLocations { get; set; } = Enumerable.Empty<MemberNotificationLocationsModel>();
    public bool? ReceiveMonthlyNotifications { get; set; }
}
public class MemberNotificationEventFormatModel
{
    public long Id { get; set; }
    public Guid MemberId { get; set; }
    public string EventFormat { get; set; } = string.Empty;
    public int Ordering { get; set; }
    public bool ReceiveNotifications { get; set; }
}

public class MemberNotificationLocationsModel
{
    public long Id { get; set; }
    public Guid MemberId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Radius { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
namespace SFA.DAS.ApprenticeAan.Application.MemberNotificationSettings.Queries.GetMemberNotificationSettings;

public class GetMemberNotificationSettingsQueryResult
{
    public IEnumerable<MemberNotificationEventFormatModel> MemberNotificationEventFormats { get; set; } = Enumerable.Empty<MemberNotificationEventFormatModel>();
    public IEnumerable<MemberNotificationLocationsModel> MemberNotificationLocations { get; set; } = Enumerable.Empty<MemberNotificationLocationsModel>();
    public bool? ReceiveMonthlyNotifications { get; set; }
}

public class MemberNotificationEventFormatModel
{
}

public class MemberNotificationLocationsModel
{
}
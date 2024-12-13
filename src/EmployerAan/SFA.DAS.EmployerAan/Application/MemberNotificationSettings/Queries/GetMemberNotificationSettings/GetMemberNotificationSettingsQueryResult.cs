using SFA.DAS.EmployerAan.Application.MemberNotificationEventFormat.Queries.GetMemberNotificationEventFormats;
using SFA.DAS.EmployerAan.Application.MemberNotificationLocations.Queries;

namespace SFA.DAS.EmployerAan.Application.MemberNotificationSettings.Queries.GetMemberNotificationSettings;

public class GetMemberNotificationSettingsQueryResult
{
    public IEnumerable<MemberNotificationEventFormatModel> MemberNotificationEventFormats { get; set; } = Enumerable.Empty<MemberNotificationEventFormatModel>();
    public IEnumerable<MemberNotificationLocationsModel> MemberNotificationLocations { get; set; } = Enumerable.Empty<MemberNotificationLocationsModel>();
    public bool ReceiveMonthlyNotifications { get; set; }
}

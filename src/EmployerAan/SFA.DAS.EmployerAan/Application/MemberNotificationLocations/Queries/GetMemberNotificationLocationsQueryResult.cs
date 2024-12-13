namespace SFA.DAS.EmployerAan.Application.MemberNotificationLocations.Queries;

public class GetMemberNotificationLocationsQueryResult
{
    public IEnumerable<MemberNotificationLocationsModel> MemberNotificationLocations { get; set; } = Enumerable.Empty<MemberNotificationLocationsModel>();
}

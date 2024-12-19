namespace SFA.DAS.EmployerAan.Application.MemberNotificationSettings.Queries.GetMemberNotificationSettings;

public class MemberNotificationLocationsModel
{
    public long Id { get; set; }
    public Guid MemberId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Radius { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

namespace SFA.DAS.ApprenticeAan.Application.Entities;

public class MemberProfile
{
    public int ProfileId { get; set; }
    public string Value { get; set; } = string.Empty;
    public int? PreferenceId { get; set; }
}
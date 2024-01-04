namespace SFA.DAS.ApprenticeAan.Application.Models;

public class MemberProfile
{
    public int ProfileId { get; set; }
    public string Value { get; set; } = null!;
    public int? PreferenceId { get; set; }
}
namespace SFA.DAS.ApprenticeAan.Application.InnerApi.MemberProfiles;

public class UpdateMemberProfileModel
{
    public List<UpdateProfileModel> MemberProfiles { get; set; } = new List<UpdateProfileModel>();
    public List<UpdatePreferenceModel> Preferences { get; set; } = new List<UpdatePreferenceModel>();
}

public class UpdateProfileModel
{
    public int ProfileId { get; set; }
    public string? Value { get; set; }
}
public class UpdatePreferenceModel
{
    public int PreferenceId { get; set; }
    public bool Value { get; set; }
}
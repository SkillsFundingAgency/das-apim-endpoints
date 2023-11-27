namespace SFA.DAS.ApprenticeAan.Application.InnerApi.MemberProfiles;

public class UpdateProfileModel
{
    public int MemberProfileId { get; set; }
    public string? Value { get; set; }
}
public class UpdatePreferenceModel
{
    public int PreferenceId { get; set; }
    public bool Value { get; set; }
}

public class UpdateMemberProfileRequest
{
    public List<UpdateProfileModel> MemberProfiles { get; set; } = new List<UpdateProfileModel>();
    public List<UpdatePreferenceModel> MemberPreferences { get; set; } = new List<UpdatePreferenceModel>();
}

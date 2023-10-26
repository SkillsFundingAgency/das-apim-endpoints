namespace SFA.DAS.ApprenticeAan.Application.InnerApi.MemberProfiles;


public class UpdateMemberProfileModel
{
    public ProfilesAndPreferencesModel Model { get; set; } = null!;
}
public class ProfilesAndPreferencesModel
{
    public List<UpdateProfileModel> Profiles { get; set; } = new List<UpdateProfileModel>();
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
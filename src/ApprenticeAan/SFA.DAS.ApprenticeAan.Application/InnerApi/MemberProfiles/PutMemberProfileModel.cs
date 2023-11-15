namespace SFA.DAS.ApprenticeAan.Application.InnerApi.MemberProfiles;

public class UpdateMemberProfileModel
{
    public List<UpdateProfileModel> Profiles { get; set; } = new List<UpdateProfileModel>();
    public List<UpdatePreferenceModel> Preferences { get; set; } = new List<UpdatePreferenceModel>();
}


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

public class UpdateMemberProfileCommand
{
    public List<UpdateProfileMappedModel> Profiles { get; set; } = new List<UpdateProfileMappedModel>();
    public List<UpdatePreferenceModel> Preferences { get; set; } = new List<UpdatePreferenceModel>();

    public static implicit operator UpdateMemberProfileCommand(UpdateMemberProfileModel source) =>
        new()
        {
            Preferences = source.Preferences,
            Profiles = source.Profiles.Select(x => (UpdateProfileMappedModel)x).ToList()
        };
}

public class UpdateProfileMappedModel
{
    public int ProfileId { get; set; }
    public string? Value { get; set; }

    public static implicit operator UpdateProfileMappedModel(UpdateProfileModel source) =>
        new()
        {
            ProfileId = source.MemberProfileId,
            Value = source.Value
        };
}
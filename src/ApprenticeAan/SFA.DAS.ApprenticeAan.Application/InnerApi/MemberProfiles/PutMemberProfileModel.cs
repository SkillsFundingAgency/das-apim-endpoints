namespace SFA.DAS.ApprenticeAan.Application.InnerApi.MemberProfiles;
public class UpdateMemberProfileModel
{
    public IEnumerable<UpdateProfileModel> Profiles { get; set; } = Enumerable.Empty<UpdateProfileModel>();
    public IEnumerable<UpdatePreferenceModel> Preferences { get; set; } = Enumerable.Empty<UpdatePreferenceModel>();
}


public class UpdateProfileModel
{
    public int ProfileId { get; set; }
    public string? Value { get; set; } = null!;
}
public class UpdatePreferenceModel
{
    public int PreferenceId { get; set; }
    public bool Value { get; set; }
}
namespace SFA.DAS.ApprenticeAan.Application.InnerApi.MemberProfiles;

public class UpdateProfileModel
{
    public int Id { get; set; }
    public string? Value { get; set; }
}
public class UpdatePreferenceModel
{
    public int Id { get; set; }
    public bool Value { get; set; }
}

public class UpdateMemberProfileRequest
{
    public IEnumerable<UpdateProfileModel> Profiles { get; set; } = Enumerable.Empty<UpdateProfileModel>();
    public IEnumerable<UpdatePreferenceModel> Preferences { get; set; } = Enumerable.Empty<UpdatePreferenceModel>();
}

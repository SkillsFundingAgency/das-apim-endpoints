namespace SFA.DAS.AdminAan.Domain;

public record GetMemberProfilesAndPreferencesResponse(IEnumerable<MemberProfileModel> Profiles, IEnumerable<MemberPreferenceModel> Preferences);

public record MemberProfileModel(int ProfileId, string Value, int? PreferenceId);

public record MemberPreferenceModel(int PreferenceId, bool Value);

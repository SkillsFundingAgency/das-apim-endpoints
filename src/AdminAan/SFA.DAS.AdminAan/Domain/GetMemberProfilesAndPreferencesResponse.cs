namespace SFA.DAS.AdminAan.Domain;

public record GetMemberProfilesAndPreferencesResponse(IEnumerable<MemberProfile> Profiles, IEnumerable<MemberPreference> Preferences);

public record MemberProfile(int ProfileId, string Value, int? PreferenceId);

public record MemberPreference(int PreferenceId, bool Value);

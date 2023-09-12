using SFA.DAS.ApprenticeAan.Application.Entities;

namespace SFA.DAS.ApprenticeAan.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;
public class GetMemberProfileWithPreferencesQueryResult
{
    public IEnumerable<MemberProfile> Profiles { get; set; } = Enumerable.Empty<MemberProfile>();
    public IEnumerable<MemberPreference> Preferences { get; set; } = Enumerable.Empty<MemberPreference>();
}


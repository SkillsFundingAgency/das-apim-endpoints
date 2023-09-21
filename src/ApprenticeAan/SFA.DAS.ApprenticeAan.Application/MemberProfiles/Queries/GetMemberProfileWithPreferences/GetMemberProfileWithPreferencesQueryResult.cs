using SFA.DAS.ApprenticeAan.Application.Common;
using SFA.DAS.ApprenticeAan.Application.Entities;

namespace SFA.DAS.ApprenticeAan.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;
public class GetMemberProfileWithPreferencesQueryResult
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int? RegionId { get; set; }
    public string RegionName { get; set; } = string.Empty;
    public MemberUserType UserType { get; set; }
    public bool IsRegionalChair { get; set; }
    public Apprenticeship Apprenticeship { get; set; } = null!;
    public IEnumerable<MemberProfile> Profiles { get; set; } = Enumerable.Empty<MemberProfile>();
    public IEnumerable<MemberPreference> Preferences { get; set; } = Enumerable.Empty<MemberPreference>();
}

public class Apprenticeship
{
    public string Sector { get; set; } = string.Empty;
    public string Programmes { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
}
using SFA.DAS.EmployerAan.Common;
using SFA.DAS.EmployerAan.Models;

namespace SFA.DAS.EmployerAan.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;

public class GetMemberProfileWithPreferencesQueryResult
{
    public Guid ApprenticeId { get; set; }
    public long AccountId { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? OrganisationName { get; set; }
    public int RegionId { get; set; }
    public string RegionName { get; set; } = null!;
    public MemberUserType UserType { get; set; }
    public bool IsRegionalChair { get; set; }
    public IEnumerable<MemberProfile> Profiles { get; set; } = Enumerable.Empty<MemberProfile>();
    public IEnumerable<MemberPreference> Preferences { get; set; } = Enumerable.Empty<MemberPreference>();
}

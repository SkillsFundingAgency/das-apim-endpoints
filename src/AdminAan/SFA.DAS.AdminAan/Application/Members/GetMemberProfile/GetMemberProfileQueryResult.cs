using SFA.DAS.AdminAan.Domain;

namespace SFA.DAS.AdminAan.Application.Members.GetMemberProfile;

public class GetMemberProfileQueryResult
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? OrganisationName { get; set; }
    public int? RegionId { get; set; }
    public string RegionName { get; set; } = null!;
    public MemberUserType UserType { get; set; }
    public bool IsRegionalChair { get; set; }
    public Apprenticeship Apprenticeship { get; set; } = new();
    public IEnumerable<MemberProfile> Profiles { get; set; } = Enumerable.Empty<MemberProfile>();
    public IEnumerable<MemberPreference> Preferences { get; set; } = Enumerable.Empty<MemberPreference>();
}

public class Apprenticeship
{
    public string? Sector { get; set; } = null!;
    public string? Programme { get; set; } = null!;
    public string? Level { get; set; } = null!;
    public int? ActiveApprenticesCount { get; set; } = null!;
    public IEnumerable<string> Sectors { get; set; } = Enumerable.Empty<string>();
}

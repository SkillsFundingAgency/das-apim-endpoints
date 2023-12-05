namespace SFA.DAS.AdminAan.Domain;

public class GetMembersRequest
{
    public string? Keyword { get; set; }

    public List<int> RegionId { get; set; } = new List<int>();

    public List<MemberUserType> UserType { get; set; } = new List<MemberUserType>();

    public bool? IsRegionalChair { get; set; }

    public int? Page { get; set; }

    public int? PageSize { get; set; }
}

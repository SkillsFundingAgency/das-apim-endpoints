namespace SFA.DAS.ApprenticeAan.Application.Members.Queries.GetMembers;

public class MembersSummary
{
    public Guid MemberId { get; set; }
    public string? FullName { get; set; } = null!;
    public int? RegionId { get; set; } = null!;
    public string? RegionName { get; set; } = null!;
    public string UserType { get; set; } = "";
    public bool? IsRegionalChair { get; set; } = null!;
    public DateTime JoinedDate { get; set; }
}

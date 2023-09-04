namespace SFA.DAS.EmployerAan.Application.Members.Queries.GetMembers;
public class MemberSummary
{
    public Guid MemberId { get; set; }
    public string? FullName { get; set; } = null!;
    public int? RegionId { get; set; } = null!;
    public string? RegionName { get; set; } = null!;
    public string UserType { get; set; } = "";
    public bool? IsRegionalChair { get; set; } = null!;
    public DateTime JoinedDate { get; set; }
}

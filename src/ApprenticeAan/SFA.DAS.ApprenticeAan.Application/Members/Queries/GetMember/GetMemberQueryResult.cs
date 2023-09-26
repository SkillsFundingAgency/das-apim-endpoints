namespace SFA.DAS.ApprenticeAan.Application.Members.Queries.GetMember;

public class GetMemberQueryResult
{
    public Guid MemberId { get; set; }
    public Guid? ApprenticeId { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string? OrganisationName { get; set; }
    public int? RegionId { get; set; }
    public string UserType { get; set; } = null!;
    public DateTime JoinedDate { get; set; }
    public bool? IsRegionalChair { get; set; }
    public string FullName { get; set; } = null!;
}

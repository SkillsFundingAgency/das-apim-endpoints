namespace SFA.DAS.EmployerAan.Application.Members.Queries.GetMemberByMemberId;

public class GetMemberByIdQueryResult
{
    public Guid MemberId { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? OrganisationName { get; set; }
    public int? RegionId { get; set; }
    public string UserType { get; set; } = null!;
    public bool? IsRegionalChair { get; set; }
    public string FullName { get; set; } = null!;
    public long EmployerAccountId { get; set; }
    public Guid UserRef { get; set; }
}
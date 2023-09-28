namespace SFA.DAS.EmployerAan.Application.Members.Queries.GetMember;

public class GetMemberQueryResult
{
    public Guid MemberId { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string? OrganisationName { get; set; }
    public int? RegionId { get; set; }
    public string UserType { get; set; } = null!;
    public DateTime JoinedDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime LastUpdatedDate { get; set; }
    public bool IsRegionalChair { get; set; }
    public string FullName { get; set; } = null!;
    public EmployerModel? Employer { get; set; }
    public ApprenticeModel? Apprentice { get; set; }

}

public record EmployerModel(long AccountId, Guid UserRef);

public record ApprenticeModel(Guid ApprenticeId);

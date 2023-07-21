using MediatR;

namespace SFA.DAS.EmployerAan.Application.Employer.Commands.CreateEmployerMember;

public class CreateEmployerMemberCommand : IRequest<CreateEmployerMemberCommandResult>
{
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime JoinedDate { get; set; }
    public int? RegionId { get; set; }
    public string? OrganisationName { get; set; } = null!;
    public List<ProfileValue> ProfileValues { get; set; } = new();
    public long AccountId { get; init; }
    public Guid UserRef { get; init; }
}
public record class ProfileValue(int Id, string Value);
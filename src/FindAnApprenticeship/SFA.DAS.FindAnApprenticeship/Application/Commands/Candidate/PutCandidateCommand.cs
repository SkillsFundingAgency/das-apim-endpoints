using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Candidate;
public class PutCandidateCommand : IRequest<PutCandidateCommandResult>
{
    public string GovUkIdentifier { get; set; } = null!;
    public string Email { get; set; } = null!;
}

using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Candidate
{
    public class CreateCandidateCommand : IRequest<CreateCandidateCommandResult>
    {
        public string GovUkIdentifier { get; set; }
        public string Email { get; set; }
    }
}

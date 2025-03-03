using MediatR;
using SFA.DAS.FindAnApprenticeship.Domain.Models;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Users.Status
{
    public record UpdateCandidateStatusCommand : IRequest<Unit>
    {
        public string GovUkIdentifier { get; set; }
        public string Email { get; set; }
        public UserStatus Status { get; set; }
    }
}

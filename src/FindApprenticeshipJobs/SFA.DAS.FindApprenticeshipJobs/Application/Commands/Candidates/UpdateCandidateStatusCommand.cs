using MediatR;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Commands.Candidates
{
    public record UpdateCandidateStatusCommand : IRequest
    {
        public string GovUkIdentifier { get; set; }
        public string Email { get; set; }
        public UserStatus Status { get; set; }
    }
}
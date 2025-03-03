using MediatR;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Commands.Candidates
{
    public record UpdateCandidateStatusCommand : IRequest
    {
        public required string GovUkIdentifier { get; set; }
        public required string Email { get; set; }
        public UserStatus Status { get; set; }
    }
}
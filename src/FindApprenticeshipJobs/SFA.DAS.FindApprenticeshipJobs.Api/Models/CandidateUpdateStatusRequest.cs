using SFA.DAS.FindApprenticeshipJobs.Domain.Models;

namespace SFA.DAS.FindApprenticeshipJobs.Api.Models
{
    public class CandidateUpdateStatusRequest
    {
        public required string Email { get; set; }
        public UserStatus Status { get; set; }
    }
}

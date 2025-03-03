using SFA.DAS.FindApprenticeshipJobs.Domain.Models;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses
{
    public class GetCandidateApiResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleNames { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public UserStatus Status { get; set; }
    }
}

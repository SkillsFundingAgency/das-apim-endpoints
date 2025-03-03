using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests
{
    public class PutCandidateApiRequest(Guid candidateId, PutCandidateApiRequestData data) : IPutApiRequest
    {
        public object Data { get; set; } = data;

        public string PutUrl => $"/api/candidates/{candidateId}";
    }
    public class PutCandidateApiRequestData
    {
        public string Email { get; set; }
        public UserStatus? Status { get; set; }
    }
}

using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests
{
    public class GetCandidateApiRequest(string candidateId) : IGetApiRequest
    {
        public string GetUrl => $"api/candidates/{candidateId}";
    }
}
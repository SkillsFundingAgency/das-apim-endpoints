using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests
{
    public class GetCandidatesByActivityApiRequest(DateTime cutOffDateTime) : IGetApiRequest
    {
        public string GetUrl => $"api/candidates/GetCandidatesByActivity?cutOffDateTime={cutOffDateTime:O}";
    }
}
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests
{
    public class GetCandidatesByActivityApiRequest(string cutOffDateTime, int pageNumber, int pageSize) : IGetApiRequest
    {
        public string GetUrl => $"api/candidates/GetCandidatesByActivity?cutOffDateTime={cutOffDateTime}&pageNumber={pageNumber}&pageSize={pageSize}";
    }
}
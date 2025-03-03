using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests
{
    public class GetInactiveCandidatesApiRequest(string cutOffDateTime, int pageNumber, int pageSize) : IGetApiRequest
    {
        public string GetUrl => $"api/candidates/GetInactiveCandidates?cutOffDateTime={cutOffDateTime}&pageNumber={pageNumber}&pageSize={pageSize}";
    }
}
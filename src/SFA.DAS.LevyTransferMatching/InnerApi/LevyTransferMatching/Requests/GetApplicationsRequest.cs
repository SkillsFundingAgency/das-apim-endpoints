using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests
{
    public class GetApplicationsRequest : IGetApiRequest
    {
        public GetApplicationsRequest(int pledgeId)
        {
            PledgeId = pledgeId;
        }

        public int PledgeId { get; }

        public string GetUrl => $"pledges/{PledgeId}/applications";
    }
}

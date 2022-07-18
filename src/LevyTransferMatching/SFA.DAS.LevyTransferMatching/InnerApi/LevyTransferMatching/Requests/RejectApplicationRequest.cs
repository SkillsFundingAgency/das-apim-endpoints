using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests
{
    public class RejectApplicationRequest : IPostApiRequest
    {
        public int PledgeId { get; private set; }
        public int ApplicationId { get; private set; }

        public RejectApplicationRequest(int pledgeId, int applicationId, RejectApplicationRequestData data)
        {
            PledgeId = pledgeId;
            ApplicationId = applicationId;
            Data = data;
        }

        public string PostUrl => $"/pledges/{PledgeId}/applications/{ApplicationId}/reject";
        public object Data { get; set; }
    }

    public class RejectApplicationRequestData
    {
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
    }
}

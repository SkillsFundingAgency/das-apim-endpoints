using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests
{
    public class ApproveApplicationRequest : IPostApiRequest
    {
        public int PledgeId { get; private set; }
        public int ApplicationId { get; private set; }

        public ApproveApplicationRequest(int pledgeId, int applicationId, ApproveApplicationRequestData data)
        {
            PledgeId = pledgeId;
            ApplicationId = applicationId;
            Data = data;
        }

        public string PostUrl => $"/pledges/{PledgeId}/applications/{ApplicationId}/approve";
        public object Data { get; set; }
    }

    public class ApproveApplicationRequestData
    {
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public bool AutomaticApproval { get; set; }
    }
}

using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests
{
    public class CreditPledgeRequest : IPostApiRequest
    {
        private readonly long _pledgeId;

        public CreditPledgeRequest(long pledgeId, CreditPledgeRequestData data)
        {
            _pledgeId = pledgeId;
            Data = data;
        }

        public string PostUrl => $"pledges/{_pledgeId}/credit";

        public object Data { get; set; }

        public class CreditPledgeRequestData
        {
            public int Amount { get; set; }
            public int ApplicationId { get; set; }
        }
    }
}
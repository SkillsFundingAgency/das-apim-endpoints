using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests
{
    public class DebitPledgeRequest : IPostApiRequest
    {
        private readonly long _pledgeId;

        public DebitPledgeRequest(long pledgeId, DebitPledgeRequestData data)
        {
            _pledgeId = pledgeId;
            Data = data;
        }

        public string PostUrl => $"pledges/{_pledgeId}/debit";

        public object Data { get; set; }

        public class DebitPledgeRequestData
        {
            public int Amount { get; set; }
            public int ApplicationId { get; set; }
        }
    }
}

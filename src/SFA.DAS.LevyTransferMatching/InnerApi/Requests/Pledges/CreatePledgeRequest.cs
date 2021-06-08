using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.Requests.Pledges
{
    public class CreatePledgeRequest : IPostApiRequest
    {
        private readonly string _encodedAccountId;

        public CreatePledgeRequest(string encodedAccountId)
        {
            _encodedAccountId = encodedAccountId;
        }

        public string PostUrl => $"accounts/{_encodedAccountId}/pledges";

        public object Data { get; set; }
    }
}

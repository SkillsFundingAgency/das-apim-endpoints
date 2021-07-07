using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.Requests.Pledges
{
    public class CreatePledgeRequest : IPostApiRequest
    {
        private readonly long _accountId;

        public CreatePledgeRequest(long accountId)
        {
            _accountId = accountId;
        }

        public string PostUrl => $"accounts/{_accountId}/pledges";

        public object Data { get; set; }
    }
}

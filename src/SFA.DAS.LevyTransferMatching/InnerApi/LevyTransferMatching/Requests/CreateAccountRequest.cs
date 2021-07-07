using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests
{
    public class CreateAccountRequest : IPostApiRequest
    {
        public long AccountId { get; }
        public string AccountName { get;  }

        public CreateAccountRequest(long accountId, string accountName)
        {
            AccountId = accountId;
            AccountName = accountName;
        }

        public string PostUrl => "/accounts";
        public object Data { get; set; }
    }
}

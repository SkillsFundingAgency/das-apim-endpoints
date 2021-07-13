using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests
{
    public class CreateAccountRequest : IPostApiRequest
    {
        public CreateAccountRequest(long accountId, string accountName)
        {
            Data = new CreateAccountRequestData(accountId,accountName);
        }

        public string PostUrl => "/accounts";
        public object Data { get; set; }

        public class CreateAccountRequestData
        {
            public long AccountId { get; }
            public string AccountName { get; }

            public CreateAccountRequestData(long accountId, string accountName)
            {
                AccountId = accountId;
                AccountName = accountName;
            }
        }
    }
}

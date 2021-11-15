using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests
{
    public class DeclineFundingRequest : IPostApiRequest
    {
        public DeclineFundingRequest(int applicationId, long accountId, DeclineFundingRequestData data)
        {
            ApplicationId = applicationId;
            AccountId = accountId;
            Data = data;
        }

        public int ApplicationId { get; }
        public long AccountId { get; }
        public string PostUrl => $"/accounts/{AccountId}/applications/{ApplicationId}/decline-funding";
        public object Data { get; set; }
    }

    public class DeclineFundingRequestData
    {
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public int ApplicationId { get; set; }
        public long AccountId { get; set; }
    }
}
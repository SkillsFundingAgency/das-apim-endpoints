using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests
{
    public class AcceptFundingRequest : IPostApiRequest
    {
        public AcceptFundingRequest(int applicationId, long accountId, AcceptFundingRequestData data)
        {
            ApplicationId = applicationId;
            AccountId = accountId;
            Data = data;
        }

        public int ApplicationId { get; }
        public long AccountId { get; }
        public string PostUrl => $"/accounts/{AccountId}/applications/{ApplicationId}/accept-funding";
        public object Data { get; set; }
    }

    public class AcceptFundingRequestData
    {
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public int ApplicationId { get; set; }
        public long AccountId { get; set; }
    }
}

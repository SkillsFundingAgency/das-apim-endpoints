using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests
{
    public class WithdrawApplicationAfterAcceptanceRequest : IPostApiRequest
    {
        public WithdrawApplicationAfterAcceptanceRequest(long accountId, int applicationId, WithdrawApplicationAfterAcceptanceRequestData data)
        {
            AccountId = accountId;
            ApplicationId = applicationId;
            Data = data;
        }

        public long AccountId { get; set; }
        public int ApplicationId { get; set; }

        public string PostUrl => $"/accounts/{AccountId}/applications/{ApplicationId}/withdraw-after-acceptance";

        public object Data { get; set; }
    }

    public class WithdrawApplicationAfterAcceptanceRequestData
    {
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
    }
}

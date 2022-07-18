using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests
{
    public class WithdrawApplicationRequest : IPostApiRequest
    {
        public WithdrawApplicationRequest(int applicationId, long accountId, WithdrawApplicationRequestData data)
        {
            ApplicationId = applicationId;
            AccountId = accountId;
            Data = data;
        }

        public int ApplicationId { get; }
        public long AccountId { get; }
        public string PostUrl => $"/accounts/{AccountId}/applications/{ApplicationId}/withdraw";
        public object Data { get; set; }
    }

    public class WithdrawApplicationRequestData
    {
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public int ApplicationId { get; set; }
        public long AccountId { get; set; }
    }
}

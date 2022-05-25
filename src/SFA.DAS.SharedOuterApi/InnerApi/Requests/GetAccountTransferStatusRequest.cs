using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetAccountTransferStatusRequest : IGetApiRequest
    {
        public long AccountId { get; }

        public GetAccountTransferStatusRequest(long accountId)
        {
            AccountId = accountId;
        }

        public string GetUrl => $"api/accounts/{AccountId}/transfer-status";
    }
}

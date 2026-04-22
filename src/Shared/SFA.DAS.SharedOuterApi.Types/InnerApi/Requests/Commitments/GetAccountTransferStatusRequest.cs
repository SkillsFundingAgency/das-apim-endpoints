using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Commitments
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

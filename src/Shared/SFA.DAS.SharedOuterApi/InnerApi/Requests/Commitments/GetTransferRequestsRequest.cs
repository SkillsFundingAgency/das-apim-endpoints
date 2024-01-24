using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Commitments
{
    public class GetTransferRequestsRequest : IGetApiRequest
    {
        public long AccountId { get; }

        public GetTransferRequestsRequest(long accountId)
        {
            AccountId = accountId;
        }

        public string GetUrl => $"api/accounts/{AccountId}/transfers";
    }
}
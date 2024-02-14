using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Commitments
{
    public class GetTransferRequestsRequest : IGetApiRequest
    {
        public long AccountId { get; }
        public TransferType? Originator { get; }


        public GetTransferRequestsRequest(long accountId, TransferType? originator)
        {
            AccountId = accountId;
            Originator = originator;
        }

        public string GetUrl => BuildUrl();

        private string BuildUrl()
        {
            var url = $"api/accounts/{AccountId}/transfers";
            if (Originator.HasValue)
            {
                url += $"?originator={Originator}";
            }
            return url;
        }
    }
}
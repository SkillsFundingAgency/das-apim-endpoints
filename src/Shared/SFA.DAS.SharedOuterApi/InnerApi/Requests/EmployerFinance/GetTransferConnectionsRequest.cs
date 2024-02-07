using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerFinance
{
    public class GetTransferConnectionsRequest : IGetApiRequest
    {
        public long AccountId { get; set; }
        public TransferConnectionInvitationStatus? Status { get; set; }

        public string GetUrl => BuildUrl();

        private string BuildUrl()
        {
            var url = $"api/accounts/internal/{AccountId}/transfers/connections";
            if (Status.HasValue)
            {
                url += $"?status={Status}";
            }
            return url;
        }
    }
    public enum TransferConnectionInvitationStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3
    }
}
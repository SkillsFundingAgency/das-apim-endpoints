using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerFinance
{
    public class GetTransferConnectionsRequest : IGetApiRequest
    {
        private readonly long _accountId;
        private readonly TransferConnectionInvitationStatus? _status;

        public GetTransferConnectionsRequest(long accountId, TransferConnectionInvitationStatus status)
        {
            _accountId = accountId;
            _status = status;
        }

        public string GetUrl => BuildUrl();

        private string BuildUrl()
        {
            var url = $"api/accounts/internal/{_accountId}/transfers/connections";
            if (_status != null)
            {
                url += $"?status={_status}";
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
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Reservations.InnerApi.Requests
{
    public class GetTransferConnectionsRequest : IGetAllApiRequest
    {
        private readonly long _accountId;

        public GetTransferConnectionsRequest(long accountId)
        {
            _accountId = accountId;
        }

        public string GetAllUrl => $"api/accounts/internal/{_accountId}/transfers/connections";
    }
}

using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Reservations.InnerApi.Requests
{
    public class GetTransferConnectionsRequest(long accountId) : IGetAllApiRequest
    {
        public string GetAllUrl => $"api/accounts/internal/{accountId}/transfers/connections";
    }
}

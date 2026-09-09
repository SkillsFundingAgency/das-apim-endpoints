using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Reservations.InnerApi.Requests
{
    public class GetTransferConnectionsRequest(long accountId) : IGetAllApiRequest
    {
        public string GetAllUrl => $"api/accounts/internal/{accountId}/transfers/connections";
    }
}

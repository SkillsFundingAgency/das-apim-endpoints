using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class GetAccountReservationsRequest(long accountId) : IGetApiRequest
{
    public string GetUrl => $"api/accounts/{accountId}/reservations";
}

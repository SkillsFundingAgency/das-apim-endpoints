using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class GetAccountReservationsRequest : IGetAllApiRequest
{
    public readonly long AccountId;

    public GetAccountReservationsRequest(long accountId)
    {
        AccountId = accountId;
    }

    public string GetAllUrl => $"api/accounts/{AccountId}/reservations";
}


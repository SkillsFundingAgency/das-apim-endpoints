using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests;
public class GetAccountReservationsStatusRequest : IGetApiRequest
{
    public readonly long AccountId;
    public readonly long? TransferSenderId;

    public GetAccountReservationsStatusRequest(long accountId)
    {
        AccountId = accountId;
    }

    public string GetUrl => $"api/accounts/{AccountId}/status";
}
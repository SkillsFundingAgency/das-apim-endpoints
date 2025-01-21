using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests;
public class GetAccountReservationsStatusRequest : IGetApiRequest
{
    public readonly long AccountId;
    public readonly long? TransferSenderId;

    public GetAccountReservationsStatusRequest(long accountId, long? transferSenderId)
    {
        AccountId = accountId;
        TransferSenderId = transferSenderId;
    }

    public string GetUrl => $"api/accounts/{AccountId}/status?transferSenderId={TransferSenderId}";
}
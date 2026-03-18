using SFA.DAS.SharedOuterApi.Types.Models;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Commitments;

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
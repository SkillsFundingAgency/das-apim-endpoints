using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Models;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Commitments;

public class GetTransferRequestsRequest(long accountId, TransferType? originator) : IGetApiRequest
{
    public long AccountId { get; } = accountId;
    public TransferType? Originator { get; } = originator;


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
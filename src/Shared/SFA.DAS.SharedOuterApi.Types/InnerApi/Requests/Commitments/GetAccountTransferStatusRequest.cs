using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Commitments;

public class GetAccountTransferStatusRequest(long accountId) : IGetApiRequest
{
    public long AccountId { get; } = accountId;

    public string GetUrl => $"api/accounts/{AccountId}/transfer-status";
}
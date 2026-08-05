using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Commitments;

public class GetPendingApprenticeChangesRequest(long accountId) : IGetApiRequest
{
    public long AccountId { get; } = accountId;

    public string GetUrl => $"api/accounts/{AccountId}/pending-apprentice-changes";
}
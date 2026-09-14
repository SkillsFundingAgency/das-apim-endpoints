using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class GetAllChangeHistoryForProviderRequest(long providerId) : IGetApiRequest
{
    public long ProviderId { get; } = providerId;
    public string GetUrl => $"api/change-history/{ProviderId}/Get-all-change-history";
}
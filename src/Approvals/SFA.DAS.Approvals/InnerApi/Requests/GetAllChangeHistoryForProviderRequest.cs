using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class GetAllChangeHistoryForProviderRequest(long providerId, int pageNumber, int pageSize) : IGetApiRequest
{
    public long ProviderId { get; } = providerId;
    public int PageNumber { get; } = pageNumber;
    public int PageSize { get; } = pageSize;
    public string GetUrl => $"api/change-history/{ProviderId}/Get-all-change-history?pageNumber={PageNumber}&pageSize={PageSize}";
}
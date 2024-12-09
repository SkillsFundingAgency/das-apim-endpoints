using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Forecasting.InnerApi.Requests;

public class GetApprenticeshipsRequest(long accountId, string status, int pageNumber, int pageItemCount)
    : IGetApiRequest
{
    public long AccountId { get; } = accountId;
    public string Status { get; } = status;
    public int PageNumber { get; } = pageNumber;
    public int PageItemCount { get; } = pageItemCount;

    public string GetUrl => $"api/apprenticeships?accountId={AccountId}&Status={Status}&pageNumber={PageNumber}&pageItemCount={PageItemCount}";
}
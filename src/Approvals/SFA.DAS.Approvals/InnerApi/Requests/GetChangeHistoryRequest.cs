using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class GetChangeHistoryRequest(long apprenticeshipId) : IGetApiRequest
{
    public long ApprenticeshipId { get; } = apprenticeshipId;
    public string GetUrl => $"api/change-history/{ApprenticeshipId}";
}
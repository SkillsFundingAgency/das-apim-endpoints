using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class GetApprovalRequest(long apprenticeshipId, byte status) : IGetApiRequest
{
    public long ApprenticeshipId { get; } = apprenticeshipId;
    public byte Status { get; } = status;
    public string GetUrl => $"approval-requests/apprenticeships/{ApprenticeshipId}?status={Status}";
}
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class GetApprovalRequest(long apprenticeshipId, byte status, long accountId) : IGetApiRequest
{
    public long ApprenticeshipId { get; } = apprenticeshipId;
    public byte Status { get; } = status;
    public long AccountId { get; } = accountId;
    public string GetUrl => $"api/apprenticeships/{ApprenticeshipId}/approval-requests?status={Status}&accountId={AccountId}";
}
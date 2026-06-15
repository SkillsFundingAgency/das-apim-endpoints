using MediatR;

namespace SFA.DAS.Approvals.Application.ChangeHistory.Queries;

public class GetChangeHistoryQuery(long apprenticeshipId) : IRequest<GetChangeHistoryResult>
{
    public long ApprenticeshipId { get; } = apprenticeshipId;
    public string GetUrl => $"api/change-history/{ApprenticeshipId}";
}
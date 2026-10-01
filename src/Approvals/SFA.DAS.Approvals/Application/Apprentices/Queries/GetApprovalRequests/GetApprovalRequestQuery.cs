using MediatR;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.GetApprovalRequests;

public class GetApprovalRequestQuery : IRequest<GetApprovalRequestQueryResult>
{
    public long ApprenticeshipId { get; set; }
    public byte Status { get; set; }
    public long AccountId { get; set; }
}
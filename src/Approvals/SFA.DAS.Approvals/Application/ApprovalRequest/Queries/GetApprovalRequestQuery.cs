using MediatR;

namespace SFA.DAS.Approvals.Application.ApprovalRequest.Queries;

public class GetApprovalRequestQuery : IRequest<GetApprovalRequestQueryResult>
{
    public long ApprenticeshipId { get; set; }
    public byte Status { get; set; }
}
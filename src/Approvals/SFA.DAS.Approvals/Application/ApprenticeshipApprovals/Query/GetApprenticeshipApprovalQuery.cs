using System;
using MediatR;

namespace SFA.DAS.Approvals.Application.ApprenticeshipApprovals.Query;

public class GetApprenticeshipApprovalQuery : IRequest<GetApprenticeshipApprovalResponse>
{
    public long EmployerAccountId { get; set; }
    public long ApprenticeshipId { get; set; }
    public Guid ApprovalRequestId { get; set; }
}
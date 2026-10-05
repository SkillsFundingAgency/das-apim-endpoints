using System;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Application.ApprenticeshipApprovals.Commands.ProcessApprenticeshipApproval;

public class ProcessApprenticeshipApprovalCommand : IRequest<Unit>
{
    public bool ApplyChanges { get; set; }
    public long ApprenticeshipId { get; set; }
    public Guid ApprovalRequestId { get; set; }
    public UserInfo UserInfo { get; set; }
}
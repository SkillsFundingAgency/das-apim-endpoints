using System.Collections.Generic;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Application.ApprovalRequest.Commands;

public class UpdateApprovalRequestAlertAcknowledgeCommand : IRequest
{
    public long ApprenticeshipId { get; set; }
    public List<UpdateApprovalRequestAlertAcknowledgeItem> ApprovalRequestAlerts { get; set; }
}
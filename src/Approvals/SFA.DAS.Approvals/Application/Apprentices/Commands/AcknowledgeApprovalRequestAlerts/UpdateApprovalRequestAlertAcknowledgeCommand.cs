using System.Collections.Generic;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Application.Apprentices.Commands.AcknowledgeApprovalRequestAlerts;

public class UpdateApprovalRequestAlertAcknowledgeCommand : IRequest
{
    public long ApprenticeshipId { get; set; }
    public List<UpdateApprovalRequestAlertAcknowledgeItem> ApprovalRequestAlerts { get; set; }
    public long AccountId { get; set; }
}
using System.Collections.Generic;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Api.Models.Apprentices;

public class UpdateApprovalRequestAlertAcknowledgeRequest
{
    public List<UpdateApprovalRequestAlertAcknowledgeItem> ApprovalRequestAlerts { get; set; }
}
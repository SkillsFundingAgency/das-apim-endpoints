using System;
using System.Collections.Generic;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class UpdateApprovalRequestAlertAcknowledgeRequest(long apprenticeshipId, Body body) : IPutApiRequest
{
    public long ApprenticeshipId { get; set; } = apprenticeshipId;

    public string PutUrl => $"approval-requests/apprenticeships/{ApprenticeshipId}/alerts-acknowledged";

    public object Data { get; set; } = body;
}

public class Body
{
    public List<UpdateApprovalRequestAlertAcknowledgeItem> ApprovalRequestAlerts { get; set; }
}

public class UpdateApprovalRequestAlertAcknowledgeItem
{
    public Guid ApprovalRequestId { get; set; }
    public DateTime? EmployerAcknowledgedAt { get; set; }
    public string EmployerAcknowledgedBy { get; set; }
}
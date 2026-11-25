using System;
using System.Collections.Generic;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class UpdateApprovalRequestAlertAcknowledgeRequest(long apprenticeshipId, Body body) : IPutApiRequest
{
    public long ApprenticeshipId { get; set; } = apprenticeshipId;

    public string PutUrl => $"api/apprenticeships/{ApprenticeshipId}/alerts-acknowledged";

    public object Data { get; set; } = body;
}

public class Body
{
    public long AccountId { get; set; }
    public List<UpdateApprovalRequestAlertAcknowledgeItem> ApprovalRequestAlerts { get; set; }
}

public class UpdateApprovalRequestAlertAcknowledgeItem
{
    public Guid ApprovalRequestId { get; set; }
    public bool Acknowledged { get; set; }
    public UserInfo UserInfo { get; set; }   

}
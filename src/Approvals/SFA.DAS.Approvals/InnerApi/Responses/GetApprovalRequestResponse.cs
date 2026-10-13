using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Responses;

public class GetApprovalRequestResponse
{
    public string ApprenticeName { get; set; }
    public IEnumerable<ApprovalRequestItem> ApprovalRequests { get; set; }
}
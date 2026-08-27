using System.Collections.Generic;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Application.ApprovalRequest.Queries;

public class GetApprovalRequestQueryResult
{
    public string ApprenticeName { get; set; }
    public IEnumerable<ApprovalRequestItem> ApprovalRequests { get; set; }
}
using System.Collections.Generic;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.GetApprovalRequests;

public class GetApprovalRequestQueryResult
{
    public string ApprenticeName { get; set; }
    public IEnumerable<ApprovalRequestItem> ApprovalRequests { get; set; }
}
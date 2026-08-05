using System.Collections.Generic;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Application.ChangeHistory.Queries;

public class GetChangeHistoryResult
{
    public IEnumerable<GetChangeHistoryItem> ChangeHistory { get; set; }
}
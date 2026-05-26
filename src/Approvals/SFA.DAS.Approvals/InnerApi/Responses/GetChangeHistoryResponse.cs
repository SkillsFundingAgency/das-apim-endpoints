using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Responses;

public class GetChangeHistoryResponse
{
    public IEnumerable<GetChangeHistoryItemResponse> ChangeHistory { get; set; }
}
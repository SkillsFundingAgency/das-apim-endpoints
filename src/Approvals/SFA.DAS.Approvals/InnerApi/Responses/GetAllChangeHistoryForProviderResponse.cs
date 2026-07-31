using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Responses;

public class GetAllChangeHistoryForProviderResponse
{
    public IEnumerable<GetChangeHistoryItem> ChangeHistory { get; set; }
}
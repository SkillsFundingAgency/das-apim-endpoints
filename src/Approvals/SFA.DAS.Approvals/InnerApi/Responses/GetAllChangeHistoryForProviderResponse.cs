using System;
using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Responses;

public class GetAllChangeHistoryForProviderResponse
{
    public IEnumerable<GetChangeHistoryItem> ChangeHistory { get; set; }
    public int TotalCount { get; set; }
    public DateTime? AvailableFrom { get; set; }
}
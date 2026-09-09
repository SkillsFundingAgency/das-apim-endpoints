using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Responses;

public class GetAllChangeHistoryForEmployerResponse
{
    public IEnumerable<GetChangeHistoryItem> ChangeHistory { get; set; }
}
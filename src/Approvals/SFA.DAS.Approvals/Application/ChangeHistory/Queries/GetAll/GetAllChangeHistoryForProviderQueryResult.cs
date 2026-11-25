using System.Collections.Generic;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Application.ChangeHistory.Queries.GetAll;

public class GetAllChangeHistoryForProviderQueryResult
{
    public IEnumerable<GetChangeHistoryItem> ChangeHistory { get; set; }
}
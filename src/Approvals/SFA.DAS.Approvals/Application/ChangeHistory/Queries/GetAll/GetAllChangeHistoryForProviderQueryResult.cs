using System.Collections.Generic;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Application.ChangeHistory.Queries.GetAll;

public class GetAllChangeHistoryForProviderQueryResult
{
    public List<GetChangeHistoryItem> ChangeHistory { get; init; } = [];
    public int TotalCount { get; init; }
}
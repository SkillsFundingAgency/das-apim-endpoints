using System.Collections.Generic;
namespace SFA.DAS.LevyTransferMatching.Application.Queries.Functions.GetActivePledgeIdsForAccount;

public class GetActivePledgeIdsForAccountQueryResult
{
    public IEnumerable<int> PledgeIds { get; set; } = [];
    public int Page { get; set; }
    public int TotalPages { get; set; }
    public int TotalPledges { get; set; }
}

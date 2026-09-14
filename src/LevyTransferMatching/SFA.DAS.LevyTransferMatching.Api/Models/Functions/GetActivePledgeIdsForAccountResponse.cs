using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Application.Queries.Functions.GetActivePledgeIdsForAccount;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Functions;

public class GetActivePledgeIdsForAccountResponse
{
    public IEnumerable<int> PledgeIds { get; set; } = [];
    public int Page { get; set; }
    public int TotalPages { get; set; }
    public int TotalPledges { get; set; }

    public static implicit operator GetActivePledgeIdsForAccountResponse(GetActivePledgeIdsForAccountQueryResult source)
    {
        return new GetActivePledgeIdsForAccountResponse
        {
            PledgeIds = source.PledgeIds,
            Page = source.Page,
            TotalPages = source.TotalPages,
            TotalPledges = source.TotalPledges
        };
    }
}

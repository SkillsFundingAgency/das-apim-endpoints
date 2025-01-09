using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Functions;

public class ApplicationsForAutomaticExpireResult
{
    public IEnumerable<int> ApplicationIdsToExpire { get; set; }
}
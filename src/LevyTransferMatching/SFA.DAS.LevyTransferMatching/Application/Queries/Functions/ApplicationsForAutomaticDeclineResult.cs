using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Functions;

public class ApplicationsForAutomaticDeclineResult
{
    public IEnumerable<int> ApplicationIdsToDecline { get; set; }
}

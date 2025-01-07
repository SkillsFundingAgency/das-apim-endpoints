using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Application.Queries.Functions;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Functions;

public class ApplicationsForAutomaticDeclineResponse
{
    public IEnumerable<int> ApplicationIdsToDecline { get; set; }

    public static implicit operator ApplicationsForAutomaticDeclineResponse(ApplicationsForAutomaticDeclineResult source)
    {
        return new ApplicationsForAutomaticDeclineResponse
        {
            ApplicationIdsToDecline = source.ApplicationIdsToDecline
        };
    }
}

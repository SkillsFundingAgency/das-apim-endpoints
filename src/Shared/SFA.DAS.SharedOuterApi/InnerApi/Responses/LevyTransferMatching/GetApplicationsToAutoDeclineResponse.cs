using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;

public class GetApplicationsToAutoDeclineResponse
{
    public IEnumerable<int> ApplicationIdsToDecline { get; set; }
}

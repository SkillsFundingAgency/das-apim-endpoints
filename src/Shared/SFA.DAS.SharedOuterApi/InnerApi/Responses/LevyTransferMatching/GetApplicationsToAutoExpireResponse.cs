using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;

public class GetApplicationsToAutoExpireResponse
{
    public IEnumerable<int> ApplicationIdsToExpire { get; set; }
}

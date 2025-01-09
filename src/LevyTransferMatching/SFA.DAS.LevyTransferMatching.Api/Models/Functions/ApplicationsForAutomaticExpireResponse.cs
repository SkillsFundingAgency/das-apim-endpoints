using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Application.Queries.Functions;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Functions;

public class ApplicationsForAutomaticExpireResponse
{
    public IEnumerable<int> ApplicationIdsToExpire { get; set; }

    public static implicit operator ApplicationsForAutomaticExpireResponse(ApplicationsForAutomaticExpireResult source)
    {
        return new ApplicationsForAutomaticExpireResponse
        {
            ApplicationIdsToExpire = source.ApplicationIdsToExpire
        };
    }
}

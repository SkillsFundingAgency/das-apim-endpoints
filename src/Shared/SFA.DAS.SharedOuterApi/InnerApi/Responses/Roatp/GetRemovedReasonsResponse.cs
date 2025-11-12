using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
public class GetRemovedReasonsResponse
{
    public IEnumerable<RemovedReasonSummary> ReasonsForRemoval { get; set; } = Enumerable.Empty<RemovedReasonSummary>();
}
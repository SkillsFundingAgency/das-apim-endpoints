using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class GetLearnersForProviderByIdsRequest
{
    public IEnumerable<long> LearnerIds { get; set; }
}
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplications
{
    public class GetApplicationsQueryResult
    {
        public IEnumerable<SharedOuterApi.Models.Application> Applications { get; set; }
    }
}
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Shared.GetApplications
{
    public abstract class GetApplicationsQueryResultBase
    {
        public IEnumerable<SharedOuterApi.Models.Application> Applications { get; set; }
    }
}
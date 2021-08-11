using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplications
{
    public class GetApplicationsQueryResult : List<Models.Application>
    {
        public GetApplicationsQueryResult(IEnumerable<Models.Application> collection) : base(collection)
        {
                
        }
    }
}
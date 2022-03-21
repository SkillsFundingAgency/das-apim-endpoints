using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetRejectApplications
{
    public class GetRejectApplicationsQueryResult
    {
        public IEnumerable<Application> Applications { get; set; }
        public class Application
        {
            public int Id { get; set; }
            public string DasAccountName { get; set; }
        }
    }
}

using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplicationsAccountNames
{
    public class GetApplicationsAccountNamesQueryResult
    {
        public IEnumerable<Application> Applications { get; set; }
        public class Application
        {
            public int Id { get; set; }
            public string DasAccountName { get; set; }
        }
    }
}

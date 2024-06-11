using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplications;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApprovedAndAcceptedApplications
{
    public class GetApprovedAndAcceptedApplicationsResult
    {
        public IEnumerable<PledgeApplication> Applications { get; set; }
    }
}
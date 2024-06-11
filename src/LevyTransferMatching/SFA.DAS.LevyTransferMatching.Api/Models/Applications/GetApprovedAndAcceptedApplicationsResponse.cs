using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApprovedAndAcceptedApplications;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplications;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Applications
{
    public class GetApprovedAndAcceptedApplicationsResponse
    {
        public IEnumerable<PledgeApplication> Applications { get; set; }

        public static implicit operator GetApprovedAndAcceptedApplicationsResponse(GetApprovedAndAcceptedApplicationsResult source)
        {
            return new GetApprovedAndAcceptedApplicationsResponse
            {
                Applications = source.Applications
            };
        }
    }
}
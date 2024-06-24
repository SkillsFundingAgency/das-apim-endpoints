using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplications
{
    public class GetApplicationsQueryResult
    {
        public IEnumerable<PledgeApplication> Applications { get; set; }

        public string PledgeStatus { get; set; }
        public int RemainingAmount { get; set; }
        public int TotalAmount { get; set; }
        public AutomaticApprovalOption AutomaticApprovalOption { get; set; }
    }
}
using System.Linq;
using SFA.DAS.LevyTransferMatching.Infrastructure;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplications
{
    public class GetApplicationsQueryResult : PagedQueryResult<PledgeApplication>
    {
        public string PledgeStatus { get; set; }
        public int RemainingAmount { get; set; }
        public int TotalAmount { get; set; }
        public AutomaticApprovalOption AutomaticApprovalOption { get; set; }
        public int TotalPendingApplications { get; set; }
    }
}
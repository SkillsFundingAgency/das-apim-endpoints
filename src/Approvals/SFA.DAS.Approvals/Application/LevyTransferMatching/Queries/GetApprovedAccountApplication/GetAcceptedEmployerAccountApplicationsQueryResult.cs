using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.LevyTransferMatching;

namespace SFA.DAS.Approvals.Application.LevyTransferMatching.Queries.GetApprovedAccountApplication
{
    public class GetAcceptedEmployerAccountApplicationsQueryResult
    {
        public IEnumerable<GetApplicationsResponse.Application> Applications { get; set; }
    }
}
using System.Collections.Generic;
using System;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses
{
    public class GetChangeOfEmployerChainResponse
    {
        public IReadOnlyCollection<ChangeOfEmployerLink> ChangeOfEmployerChain { get; set; }

        public class ChangeOfEmployerLink
        {
            public long ApprenticeshipId { get; set; }
            public string EmployerName { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public DateTime? StopDate { get; set; }
            public DateTime? CreatedOn { get; set; }
        }
    }
}

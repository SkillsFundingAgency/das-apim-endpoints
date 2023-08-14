using System.Collections.Generic;
using System;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses
{
    public class GetChangeOfProviderChainResponse
    {
        public IReadOnlyCollection<ChangeOfProviderLink> ChangeOfProviderChain { get; set; }

        public class ChangeOfProviderLink
        {
            public long ApprenticeshipId { get; set; }
            public string ProviderName { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public DateTime? StopDate { get; set; }
            public DateTime? CreatedOn { get; set; }
        }
    }
}

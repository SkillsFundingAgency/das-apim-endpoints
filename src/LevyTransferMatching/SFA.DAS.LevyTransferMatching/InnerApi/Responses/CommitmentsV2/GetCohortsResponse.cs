using SFA.DAS.LevyTransferMatching.Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.LevyTransferMatching.InnerApi.Responses.CommitmentsV2
{
    public class GetCohortsResponse
    {
        public GetCohortsResponse()
        {
            // parameterless constructor to address system.text.json deserialisation issue.
        }

        public GetCohortsResponse(IEnumerable<CohortSummary> cohorts)
        {
            Cohorts = cohorts.ToArray();
        }
        public CohortSummary[] Cohorts { get; }

        public class CohortSummary
        {
            public long AccountId { get; set; }
            public string LegalEntityName { get; set; }
            public long ProviderId { get; set; }
            public string ProviderName { get; set; }
            public long CohortId { get; set; }
            public int NumberOfDraftApprentices { get; set; }
            public bool IsDraft { get; set; }
            public DateTime CreatedOn { get; set; }
            public long? TransferSenderId { get; set; }
            public string TransferSenderName { get; set; }
            public string AccountLegalEntityPublicHashedId { get; set; }
            public bool IsLinkedToChangeOfPartyRequest { get; set; }
            public CommitmentStatus CommitmentStatus { get; set; }
            public int? PledgeApplicationId { get; set; }
        }
    }
}

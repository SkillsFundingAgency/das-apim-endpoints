using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;

public class GetCohortsResponse
{
    public CohortSummary[] Cohorts { get; set; }

    public class CohortSummary
    {
        public long AccountId { get; set; }
        public string LegalEntityName { get; set; }
        public long ProviderId { get; set; }
        public string ProviderName { get; set; }
        public long CohortId { get; set; }
        public int NumberOfDraftApprentices { get; set; }
        public bool IsDraft { get; set; }
        public Party WithParty { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? TransferSenderId { get; set; }
        public string TransferSenderName { get; set; }
        public string AccountLegalEntityPublicHashedId { get; set; }
        public bool IsLinkedToChangeOfPartyRequest { get; set; }
        public CommitmentStatus CommitmentStatus { get; set; }
        public int? PledgeApplicationId { get; set; }
    }
}
public enum Party
{
    None = 0,
    Employer = 1,
    Provider = 2,
    TransferSender = 4
}
    
public enum CommitmentStatus
{
    New = 0,
    Active = 1,
    Deleted = 2
}
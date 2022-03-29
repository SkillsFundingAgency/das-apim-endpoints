using SFA.DAS.Approvals.Application.Shared.Enums;

namespace SFA.DAS.Approvals.Application.Cohorts.Queries
{
    public class GetCohortResult
    {
        public TransferApprovalStatus? TransferApprovalStatus { get; set; }
        public bool IsLinkedToChangeOfPartyRequest { get; set; }
        public long? ChangeOfPartyRequestId { get; set; }
        public ApprenticeshipEmployerType LevyStatus { get; set; }
        public bool IsCompleteForProvider { get; set; }
        public bool IsCompleteForEmployer { get; set; }
        public bool IsApprovedByProvider { get; set; }
        public bool IsApprovedByEmployer { get; set; }
        public LastAction LastAction { get; set; }
        public string LatestMessageCreatedByProvider { get; set; }
        public Party WithParty { get; set; }
        public int? PledgeApplicationId { get; set; }
        public long? TransferSenderId { get; set; }
        public bool IsFundedByTransfer { get; set; }
        public string ProviderName { get; set; }
        public string LegalEntityName { get; set; }
        public long AccountLegalEntityId { get; set; }
        public long CohortId { get; set; }
        public string LatestMessageCreatedByEmployer { get; set; }
        public bool ApprenticeEmailIsRequired { get; set; }
    }
}

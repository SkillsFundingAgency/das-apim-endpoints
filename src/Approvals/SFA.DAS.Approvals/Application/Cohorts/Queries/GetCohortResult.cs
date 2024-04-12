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

        public static implicit operator GetCohortResult(InnerApi.Responses.GetCohortResponse result)
        {
            return new GetCohortResult
            {
                IsLinkedToChangeOfPartyRequest = result.IsLinkedToChangeOfPartyRequest,
                TransferApprovalStatus = (Shared.Enums.TransferApprovalStatus?)result.TransferApprovalStatus,
                ChangeOfPartyRequestId = result.ChangeOfPartyRequestId,
                LevyStatus = (Shared.Enums.ApprenticeshipEmployerType)result.LevyStatus,
                IsCompleteForProvider = result.IsCompleteForProvider,
                IsCompleteForEmployer = result.IsCompleteForEmployer,
                IsApprovedByProvider = result.IsApprovedByProvider,
                IsApprovedByEmployer = result.IsApprovedByEmployer,
                LastAction = (Shared.Enums.LastAction)result.LastAction,
                LatestMessageCreatedByProvider = result.LatestMessageCreatedByProvider,
                WithParty = (Shared.Enums.Party)result.WithParty,
                PledgeApplicationId = result.PledgeApplicationId,
                TransferSenderId = result.TransferSenderId,
                IsFundedByTransfer = result.IsFundedByTransfer,
                ProviderName = result.ProviderName,
                LegalEntityName = result.LegalEntityName,
                AccountLegalEntityId = result.AccountLegalEntityId,
                CohortId = result.CohortId,
                LatestMessageCreatedByEmployer = result.LatestMessageCreatedByEmployer
            };
        }
    }
}

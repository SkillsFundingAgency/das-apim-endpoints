using System.Collections.Generic;
using System.Drawing;
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetCohortDetails;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Api.Models.Cohorts
{
    public class GetCohortDetailsResponse
    {
        public long CohortId { get; set; }
        public string CohortReference { get; set; }
        public long AccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public string LegalEntityName { get; set; }
        public bool HasNoDeclaredStandards { get; set; }
        public string ProviderName { get; set; }
        public long? ProviderId { get; set; }
        public bool IsFundedByTransfer { get; set; }
        public long? TransferSenderId { get; set; }
        public int? PledgeApplicationId { get; set; }
        public Party WithParty { get; set; }
        public string LatestMessageCreatedByEmployer { get; set; }
        public string LatestMessageCreatedByProvider { get; set; }
        public bool IsApprovedByEmployer { get; set; }
        public bool IsApprovedByProvider { get; set; }
        public bool IsCompleteForEmployer { get; set; }
        public bool IsCompleteForProvider { get; set; }
        public ApprenticeshipEmployerType LevyStatus { get; set; }
        public long? ChangeOfPartyRequestId { get; set; }
        public bool IsLinkedToChangeOfPartyRequest { get; set; }
        public TransferApprovalStatus? TransferApprovalStatus { get; set; }
        public LastAction LastAction { get; set; }
        public bool ApprenticeEmailIsRequired { get; set; }
        public bool HasUnavailableFlexiJobAgencyDeliveryModel { get; set; }
        public IEnumerable<string> InvalidProviderCourseCodes { get; set; }
        public IEnumerable<DraftApprenticeship> DraftApprenticeships { get; set; }
        public IEnumerable<ApprenticeshipEmailOverlap> ApprenticeshipEmailOverlaps { get; set; }
        public IEnumerable<long> RplErrorDraftApprenticeshipIds { get; set; }

        public static implicit operator GetCohortDetailsResponse(GetCohortDetailsQueryResult source)
        {
            return new GetCohortDetailsResponse
            {
                LegalEntityName = source.LegalEntityName,
                ProviderName = source.ProviderName,
                HasNoDeclaredStandards = source.HasNoDeclaredStandards,
                HasUnavailableFlexiJobAgencyDeliveryModel = source.HasUnavailableFlexiJobAgencyDeliveryModel,
                InvalidProviderCourseCodes = source.InvalidProviderCourseCodes,
                CohortId = source.CohortId,
                CohortReference = source.CohortReference,
                AccountId = source.AccountId,
                AccountLegalEntityId = source.AccountLegalEntityId,
                ProviderId = source.ProviderId,
                IsFundedByTransfer = source.IsFundedByTransfer,
                TransferSenderId = source.TransferSenderId,
                PledgeApplicationId = source.PledgeApplicationId,
                WithParty = source.WithParty,
                LatestMessageCreatedByEmployer = source.LatestMessageCreatedByEmployer,
                LatestMessageCreatedByProvider = source.LatestMessageCreatedByProvider,
                IsApprovedByEmployer = source.IsApprovedByEmployer,
                IsApprovedByProvider = source.IsApprovedByProvider,
                IsCompleteForEmployer = source.IsCompleteForEmployer,
                IsCompleteForProvider = source.IsCompleteForProvider,
                LevyStatus = source.LevyStatus,
                ChangeOfPartyRequestId = source.ChangeOfPartyRequestId,
                IsLinkedToChangeOfPartyRequest = source.IsLinkedToChangeOfPartyRequest,
                TransferApprovalStatus = source.TransferApprovalStatus,
                LastAction = source.LastAction,
                ApprenticeEmailIsRequired = source.ApprenticeEmailIsRequired,
                DraftApprenticeships = source.DraftApprenticeships,
                ApprenticeshipEmailOverlaps = source.ApprenticeshipEmailOverlaps,
                RplErrorDraftApprenticeshipIds = source.RplErrorDraftApprenticeshipIds
            };
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetCohortDetails;
using SFA.DAS.Approvals.InnerApi;
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
                DraftApprenticeships = source.DraftApprenticeships.Select(x=>(DraftApprenticeship)x),
                ApprenticeshipEmailOverlaps = source.ApprenticeshipEmailOverlaps.Select(x=>(ApprenticeshipEmailOverlap)x),
                RplErrorDraftApprenticeshipIds = source.RplErrorDraftApprenticeshipIds
            };
        }
    }

    public class ApprenticeshipEmailOverlap
    {
        public long Id { get; set; }

        public string ErrorMessage { get; set; }

        public static implicit operator ApprenticeshipEmailOverlap(
            InnerApi.Responses.ApprenticeshipEmailOverlap source)
        {
            return new ApprenticeshipEmailOverlap
            {
                Id = source.Id,
                ErrorMessage = source.ErrorMessage
            };
        }
    }

    public class DraftApprenticeship
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Cost { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Uln { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public DeliveryModel DeliveryModel { get; set; }
        public DateTime? OriginalStartDate { get; set; }
        public int? EmploymentPrice { get; set; }
        public DateTime? EmploymentEndDate { get; set; }
        public bool? RecognisePriorLearning { get; set; }
        public int? DurationReducedBy { get; set; }
        public int? PriceReducedBy { get; set; }
        public bool RecognisingPriorLearningStillNeedsToBeConsidered { get; set; }
        public bool RecognisingPriorLearningExtendedStillNeedsToBeConsidered { get; set; }
        public bool? IsOnFlexiPaymentPilot { get; set; }
        public bool? EmailAddressConfirmed { get; set; }
        public int? DurationReducedByHours { get; set; }
        public int? WeightageReducedBy { get; set; }
        public string QualificationsForRplReduction { get; set; }
        public string ReasonForRplReduction { get; set; }

        public static implicit operator DraftApprenticeship(
            InnerApi.Responses.DraftApprenticeship source)
        {
            return new DraftApprenticeship
            {
                Id = source.Id,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                DateOfBirth = source.DateOfBirth,
                Cost = source.Cost,
                StartDate = source.StartDate,
                ActualStartDate = source.ActualStartDate,
                EndDate = source.EndDate,
                Uln = source.Uln,
                CourseCode = source.CourseCode,
                CourseName = source.CourseName,
                DeliveryModel = source.DeliveryModel,
                OriginalStartDate = source.OriginalStartDate,
                EmploymentPrice = source.EmploymentPrice,
                EmploymentEndDate = source.EmploymentEndDate,
                RecognisePriorLearning = source.RecognisePriorLearning,
                DurationReducedBy = source.DurationReducedBy,
                PriceReducedBy = source.PriceReducedBy,
                RecognisingPriorLearningStillNeedsToBeConsidered = source.RecognisingPriorLearningStillNeedsToBeConsidered,
                RecognisingPriorLearningExtendedStillNeedsToBeConsidered = source.RecognisingPriorLearningExtendedStillNeedsToBeConsidered,
                IsOnFlexiPaymentPilot = source.IsOnFlexiPaymentPilot,
                EmailAddressConfirmed = source.EmailAddressConfirmed,
                DurationReducedByHours = source.DurationReducedByHours,
                WeightageReducedBy = source.WeightageReducedBy,
                QualificationsForRplReduction = source.QualificationsForRplReduction,
                ReasonForRplReduction = source.ReasonForRplReduction
            };
        }
    }
}
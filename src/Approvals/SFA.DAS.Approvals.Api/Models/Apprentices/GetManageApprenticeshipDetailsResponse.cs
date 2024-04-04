using System.Collections.Generic;
using System;
using SFA.DAS.Approvals.InnerApi;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Types;

namespace SFA.DAS.Approvals.Api.Models.Apprentices
{
    public class GetManageApprenticeshipDetailsResponse
    {
        public ApprenticeshipDetails Apprenticeship { get; set; }
        public IReadOnlyCollection<PriceEpisode> PriceEpisodes { get; set; }
        public IReadOnlyCollection<ApprenticeshipUpdate> ApprenticeshipUpdates { get; set; }
        public IReadOnlyCollection<DataLock> DataLocks { get; set; }
        public IReadOnlyCollection<ChangeOfPartyRequest> ChangeOfPartyRequests { get; set; }
        public IReadOnlyCollection<ChangeOfProviderLink> ChangeOfProviderChain { get; set; }
        public IReadOnlyCollection<ChangeOfEmployerLink> ChangeOfEmployerChain { get; set; }
        public IReadOnlyCollection<ApprenticeshipOverlappingTrainingDateRequest> OverlappingTrainingDateRequest { get; set; }
        public bool HasMultipleDeliveryModelOptions { get; set; }
        public PendingPriceChangeDetails PendingPriceChange { get; set; }
        public bool? CanActualStartDateBeChanged { get; set; }

		public class ApprenticeshipDetails
        {
            public long Id { get; set; }
            public long CohortId { get; set; }
            public long ProviderId { get; set; }
            public string ProviderName { get; set; }
            public long EmployerAccountId { get; set; }
            public long AccountId => EmployerAccountId;
            public long AccountLegalEntityId { get; set; }
            public string EmployerName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Uln { get; set; }
            public string CourseCode { get; set; }
            public string StandardUId { get; set; }
            public string Version { get; set; }
            public string Option { get; set; }
            public string CourseName { get; set; }
            public string DeliveryModel { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? ActualStartDate { get; set; }
            public DateTime EndDate { get; set; }
            public DateTime DateOfBirth { get; set; }
            public string EmployerReference { get; set; }
            public string ProviderReference { get; set; }
            public short Status { get; set; }
            public DateTime? StopDate { get; set; }
            public DateTime? PauseDate { get; set; }
            public DateTime? CompletionDate { get; set; }
            public string EndpointAssessorName { get; set; }
            public bool HasHadDataLockSuccess { get; set; }
            public long? ContinuationOfId { get; set; }
            public long? ContinuedById { get; set; }
            public DateTime? OriginalStartDate { get; set; }
            public long? PreviousProviderId { get; set; }
            public long? PreviousEmployerAccountId { get; set; }
            public byte? ApprenticeshipEmployerTypeOnApproval { get; set; }
            public bool? MadeRedundant { get; set; }
            public short? ConfirmationStatus { get; set; }
            public bool EmailAddressConfirmedByApprentice { get; set; }
            public bool EmailShouldBePresent { get; set; }
            public int? PledgeApplicationId { get; set; }
            public int? EmploymentPrice { get; set; }
            public DateTime? EmploymentEndDate { get; set; }
            public bool? RecognisePriorLearning { get; set; }
            public int? DurationReducedBy { get; set; }
            public int? PriceReducedBy { get; set; }
            public long? TransferSenderId { get; set; }
            public bool? IsOnFlexiPaymentPilot { get; set; }
            public int? DurationReducedByHours { get; set; }
            public int? TrainingTotalHours { get; set; }
            public bool? IsDurationReducedByRpl { get; set; }
        }

        public class PriceEpisode
        {
            public long Id { get; set; }
            public long ApprenticeshipId { get; set; }
            public decimal Cost { get; set; }
            public decimal? TrainingPrice { get; set; }
            public decimal? EndPointAssessmentPrice { get; set; }
            public DateTime FromDate { get; set; }
            public DateTime? ToDate { get; set; }
        }

        public class ApprenticeshipUpdate
        {
            public long Id { get; set; }
            public long ApprenticeshipId { get; set; }
            public short OriginatingParty { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public Decimal? Cost { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public string TrainingCode { get; set; }
            public string Version { get; set; }
            public string TrainingName { get; set; }
            public string Option { get; set; }
            public DeliveryModel? DeliveryModel { get; set; }
            public DateTime? EmploymentEndDate { get; set; }
            public int? EmploymentPrice { get; set; }
        }

        public class DataLock
        {
            public long Id { get; set; }
            public DateTime DataLockEventDatetime { get; set; }
            public string PriceEpisodeIdentifier { get; set; }
            public long ApprenticeshipId { get; set; }
            public string IlrTrainingCourseCode { get; set; }
            public DateTime? IlrActualStartDate { get; set; }
            public DateTime? IlrEffectiveFromDate { get; set; }
            public DateTime? IlrPriceEffectiveToDate { get; set; }
            public Decimal? IlrTotalCost { get; set; }
            public short ErrorCode { get; set; }
            public byte DataLockStatus { get; set; }
            public byte TriageStatus { get; set; }
            public bool IsResolved { get; set; }
        }

        public class ChangeOfPartyRequest
        {
            public long Id { get; set; }
            public ChangeOfPartyRequestType ChangeOfPartyType { get; set; }
            public short OriginatingParty { get; set; }
            public byte Status { get; set; }
            public string EmployerName { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public int? Price { get; set; }
            public long? CohortId { get; set; }
            public short? WithParty { get; set; }
            public long? NewApprenticeshipId { get; set; }
            public long? ProviderId { get; set; }
        }

        public class ChangeOfProviderLink
        {
            public long ApprenticeshipId { get; set; }
            public string ProviderName { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public DateTime? StopDate { get; set; }
            public DateTime? CreatedOn { get; set; }
        }

        public class ChangeOfEmployerLink
        {
            public long ApprenticeshipId { get; set; }
            public string EmployerName { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public DateTime? StopDate { get; set; }
            public DateTime? CreatedOn { get; set; }
        }

        public class ApprenticeshipOverlappingTrainingDateRequest
        {
            public long Id { get; set; }
            public long DraftApprenticeshipId { get; set; }
            public long PreviousApprenticeshipId { get; set; }
            public short? ResolutionType { get; set; }
            public short Status { get; set; }
            public DateTime? ActionedOn { get; set; }
        }

        public class PendingPriceChangeDetails
        {
            public decimal Cost { get; set; }
            public decimal? TrainingPrice { get; set; }
            public decimal? EndPointAssessmentPrice { get; set; }
            public DateTime? ProviderApprovedDate { get; set; }
            public DateTime? EmployerApprovedDate { get; set; }
            public string Initiator { get; set; }
        }
    }
}

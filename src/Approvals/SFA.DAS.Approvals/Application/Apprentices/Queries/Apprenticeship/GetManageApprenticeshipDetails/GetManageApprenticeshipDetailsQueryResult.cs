using System;
using System.Collections.Generic;
using SFA.DAS.Apprenticeships.Types;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using static SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.GetApprenticeshipUpdatesResponse;
using static SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.GetChangeOfEmployerChainResponse;
using static SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.GetChangeOfPartyRequestsResponse;
using static SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.GetChangeOfProviderChainResponse;
using static SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.GetOverlappingTrainingDateResponse;
using static SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.GetPriceEpisodesResponse;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetManageApprenticeshipDetails
{
    public class GetManageApprenticeshipDetailsQueryResult
    {
        public GetApprenticeshipResponse Apprenticeship{ get; set; }
        public IReadOnlyCollection<PriceEpisode> PriceEpisodes { get; set; }
        public IReadOnlyCollection<ApprenticeshipUpdate> ApprenticeshipUpdates { get; set; }
        public IReadOnlyCollection<GetDataLocksResponse.DataLock> DataLocks { get; set; }
        public IReadOnlyCollection<ChangeOfPartyRequest> ChangeOfPartyRequests { get; set; }
        public IReadOnlyCollection<ChangeOfProviderLink> ChangeOfProviderChain { get; set; }
        public IReadOnlyCollection<ChangeOfEmployerLink> ChangeOfEmployerChain { get; set; }
        public IReadOnlyCollection<ApprenticeshipOverlappingTrainingDateRequest> OverlappingTrainingDateRequest { get; set; }
        public bool HasMultipleDeliveryModelOptions { get; set; }
        public PendingPriceChange PendingPriceChange { get; set; }
        public bool? CanActualStartDateBeChanged { get; set; }
        public PendingStartDateChange PendingStartDateChange { get; set; }
        public PaymentsStatus PaymentsStatus { get; set; }
        public LearnerStatusDetails LearnerStatusDetails { get; set; }
    }

    public class PendingPriceChange
    {
        public decimal Cost { get; set; }
        public decimal? TrainingPrice { get; set; }
        public decimal? EndPointAssessmentPrice { get; set; }
        public DateTime? ProviderApprovedDate { get; set; }
        public DateTime? EmployerApprovedDate { get; set; }
        public string Initiator { get; set; }
    }

    public class PendingStartDateChange
    {
        public DateTime PendingActualStartDate { get; set; }
        public DateTime PendingPlannedEndDate { get; set; }
        public string? Initiator { get; set; }
        public DateTime? ProviderApprovedDate { get; set; }
        public DateTime? EmployerApprovedDate { get; set; }
    }

    public class PaymentsStatus
    {
        public bool PaymentsFrozen { get; set; }
        public string ReasonFrozen { get; set; }
        public DateTime? FrozenOn { get; set; }
    }

    public class LearnerStatusDetails
    {
        public LearnerStatus LearnerStatus { get; set; }
        public DateTime? WithdrawalChangedDate { get; set; }
        public string WithdrawalReason { get; set; }
        public DateTime? LastCensusDateOfLearning { get; set; }
        public DateTime? LastDayOfLearning { get; set; }
    }
}
﻿using System;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetViewDraftApprenticeship;
using SFA.DAS.Approvals.InnerApi;

namespace SFA.DAS.Approvals.Api.Models.DraftApprenticeships
{

    public class GetViewDraftApprenticeshipResponse
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Uln { get; set; }
        public string CourseCode { get; set; }
        public DeliveryModel DeliveryModel { get; set; }
        public string TrainingCourseName { get; set; }
        public string TrainingCourseVersion { get; set; }
        public string TrainingCourseOption { get; set; }
        public bool TrainingCourseVersionConfirmed { get; set; }
        public string StandardUId { get; set; }
        public int? TrainingPrice { get; set; }
        public int? EndPointAssessmentPrice { get; set; }
        public int? Cost { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Reference { get; set; }
        public string EmployerReference { get; set; }
        public string ProviderReference { get; set; }
        public Guid? ReservationId { get; set; }
        public bool IsContinuation { get; set; }
        public long? ContinuationOfId { get; set; }
        public DateTime? OriginalStartDate { get; set; }
        public bool HasStandardOptions { get; set; }
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
        public bool? IsDurationReducedByRpl { get; set; }
        public int? TrainingTotalHours { get; set; }
        public long? LearnerDataId { get; set; }
        public bool HasLearnerDataChanges { get; set; }
        public DateTime? LastLearnerDataSync { get; set; }

        public static implicit operator GetViewDraftApprenticeshipResponse(GetViewDraftApprenticeshipQueryResult source)
        {
            return new GetViewDraftApprenticeshipResponse
            {
                Id = source.Id,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                Uln = source.Uln,
                CourseCode = source.CourseCode,
                DeliveryModel = source.DeliveryModel,
                TrainingCourseName = source.TrainingCourseName,
                TrainingCourseVersion = source.TrainingCourseVersion,
                TrainingCourseOption = source.TrainingCourseOption,
                TrainingCourseVersionConfirmed = source.TrainingCourseVersionConfirmed,
                StandardUId = source.StandardUId,
                TrainingPrice = source.TrainingPrice,
                EndPointAssessmentPrice = source.EndPointAssessmentPrice,
                Cost = source.Cost,
                StartDate = source.StartDate,
                ActualStartDate = source.ActualStartDate,
                EndDate = source.EndDate,
                DateOfBirth = source.DateOfBirth,
                Reference = source.Reference,
                EmployerReference = source.EmployerReference,
                ProviderReference = source.ProviderReference,
                ReservationId = source.ReservationId,
                IsContinuation = source.IsContinuation,
                ContinuationOfId = source.ContinuationOfId,
                OriginalStartDate = source.OriginalStartDate,
                HasStandardOptions = source.HasStandardOptions,
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
                IsDurationReducedByRpl = source.IsDurationReducedByRpl,
                TrainingTotalHours = source.TrainingTotalHours
            };
        }
    }
}

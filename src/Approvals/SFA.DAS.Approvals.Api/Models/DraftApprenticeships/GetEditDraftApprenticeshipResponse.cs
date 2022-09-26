using System;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeship;
using SFA.DAS.Approvals.InnerApi;

namespace SFA.DAS.Approvals.Api.Models.DraftApprenticeships
{
    public class GetEditDraftApprenticeshipResponse
    {
        public Guid? ReservationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public string Email { get; set; }
        public string Uln { get; set; }
        public DeliveryModel DeliveryModel { get; set; }

        public string CourseCode { get; set; }
        public string StandardUId { get; set; }
        public string CourseName { get; set; }

        
        public bool HasStandardOptions { get; set; }
        public string TrainingCourseOption { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int? Cost { get; set; }

        public int? EmploymentPrice { get; set; }

        public DateTime? EmploymentEndDate { get; set; }
        public string EmployerReference { get; set; }
        public string ProviderReference { get; set; }

        public long ProviderId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public string ProviderName { get; set; }
        public string LegalEntityName { get; set; }

        public bool IsContinuation { get; set; }
        public bool HasMultipleDeliveryModelOptions { get; set; }
        public bool HasUnavailableDeliveryModel { get; set; }
        public bool? RecognisePriorLearning { get; set; }
        public int? DurationReducedBy { get; set; }
        public int? PriceReducedBy { get; set; }
        public bool RecognisingPriorLearningStillNeedsToBeConsidered { get; set; }
        public bool? IsOnFlexiPaymentPilot { get; set; }
        public DateTime? ActualStartDate { get; set; }

        public static implicit operator GetEditDraftApprenticeshipResponse(GetEditDraftApprenticeshipQueryResult source)
        {
            return new GetEditDraftApprenticeshipResponse
            {
                FirstName = source.FirstName,
                LastName = source.LastName,
                DateOfBirth = source.DateOfBirth,
                ReservationId = source.ReservationId,
                Email = source.Email,
                Uln = source.Uln,
                DeliveryModel = source.DeliveryModel,
                CourseCode = source.CourseCode,
                HasStandardOptions = source.HasStandardOptions,
                TrainingCourseOption = source.TrainingCourseOption,
                StandardUId = source.StandardUId,
                CourseName = source.CourseName,
                StartDate = source.StartDate,
                ActualStartDate = source.ActualStartDate,
                EndDate = source.EndDate,
                Cost = source.Cost,
                EmploymentPrice = source.EmploymentPrice,
                EmploymentEndDate = source.EmploymentEndDate,
                EmployerReference = source.EmployerReference,
                ProviderReference = source.ProviderReference,
                ProviderId = source.ProviderId,
                AccountLegalEntityId = source.AccountLegalEntityId,
                ProviderName = source.ProviderName,
                LegalEntityName = source.LegalEntityName,
                IsContinuation = source.IsContinuation,
                HasMultipleDeliveryModelOptions = source.HasMultipleDeliveryModelOptions,
                HasUnavailableDeliveryModel = source.HasUnavailableDeliveryModel,
                RecognisePriorLearning = source.RecognisePriorLearning,
                DurationReducedBy = source.DurationReducedBy,
                PriceReducedBy = source.PriceReducedBy,
                RecognisingPriorLearningStillNeedsToBeConsidered = source.RecognisingPriorLearningStillNeedsToBeConsidered,
                IsOnFlexiPaymentPilot = source.IsOnFlexiPaymentPilot,
                ActualStartDate = source.ActualStartDate
            };
        }
    }
}

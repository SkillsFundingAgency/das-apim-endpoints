using System;
using SFA.DAS.Approvals.InnerApi;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeship
{
    public class GetEditDraftApprenticeshipQueryResult
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
        public DateTime? EndDate { get; set; }

        public int? Cost { get; set; }

        public int? EmploymentPrice { get; set; }

        public DateTime? EmploymentEndDate { get; set; }
        public string ProviderReference { get; set; }
        public string EmployerReference { get; set; }

        public long ProviderId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public string ProviderName { get; set; }
        public string LegalEntityName { get; set; }

        public bool IsContinuation { get; set; }
        public bool HasMultipleDeliveryModelOptions { get; set; }
        public bool? RecognisePriorLearning { get; set; }
        public int? DurationReducedBy { get; set; }
        public int? PriceReducedBy { get; set; }
        public bool RecognisingPriorLearningStillNeedsToBeConsidered { get; set; }
    }
}

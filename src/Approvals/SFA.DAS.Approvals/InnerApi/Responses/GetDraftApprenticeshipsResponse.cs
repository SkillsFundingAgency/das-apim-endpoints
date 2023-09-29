using System;
using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Responses
{
    public class GetDraftApprenticeshipsResponse
    {
        public List<DraftApprenticeship> DraftApprenticeships { get; set; } = new List<DraftApprenticeship>();
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
    }
}

using System;
using MediatR;
using SFA.DAS.Approvals.InnerApi;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.UpdateDraftApprenticeship
{
    public class UpdateDraftApprenticeshipCommand : IRequest<UpdateDraftApprenticeshipResult>
    {
        public long CohortId { get; set; }
        public long ApprenticeshipId { get; set; }
        public UserInfo UserInfo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Uln { get; set; }
        public string CourseCode { get; set; }
        public string CourseOption { get; set; }
        public DeliveryModel DeliveryModel { get; set; }
        public int? EmploymentPrice { get; set; }
        public int? Cost { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? EmploymentEndDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Reference { get; set; }
        public Guid? ReservationId { get; set; }
        public bool IgnoreStartDateOverlap { get; set; }
        public bool? IsOnFlexiPaymentPilot { get; set; }
    }
}

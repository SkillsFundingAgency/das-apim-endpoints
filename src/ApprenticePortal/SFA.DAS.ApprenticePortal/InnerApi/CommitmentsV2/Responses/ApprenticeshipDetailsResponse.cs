using System;

namespace SFA.DAS.ApprenticePortal.InnerApi.CommitmentsV2.Responses
{
    public class ApprenticeshipDetailsResponse
    {
        public long Id { get; set; }
        public long EmployerAccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public string EmployerName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Uln { get; set; }
        public string StandardUId { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string Option { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? EmploymentEndDate { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? StopDate { get; set; }
        public DateTime? PauseDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public bool HasHadDataLockSuccess { get; set; }
        public DateTime? OriginalStartDate { get; set; }
        public long ProviderId { get; set; }
    }
}

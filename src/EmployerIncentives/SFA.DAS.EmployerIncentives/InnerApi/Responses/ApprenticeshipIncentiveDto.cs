using System;

namespace SFA.DAS.EmployerIncentives.InnerApi.Responses
{
    public class ApprenticeshipIncentiveDto
    {
        public Guid Id { get; set; }
        public long ApprenticeshipId { get; set; }
        public long ULN { get; set; }
        public long? UKPRN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime StartDate { get; set; }
        public string CourseName { get; set; }
    }
}

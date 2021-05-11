using System;

namespace SFA.DAS.EmployerIncentives.InnerApi.Responses.Commitments
{
    public class ApprenticeshipItem
    {
        public long Id { get; set; }
        public long Uln { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CourseName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ApprenticeshipStatus ApprenticeshipStatus { get; set; }
    }
}
using System;

namespace SFA.DAS.EmployerIncentives.Models
{
    public class ApprenticeshipItem
    {
        public long Uln { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CourseName { get; set; }
        public DateTime StartDate { get; set; }
    }
}
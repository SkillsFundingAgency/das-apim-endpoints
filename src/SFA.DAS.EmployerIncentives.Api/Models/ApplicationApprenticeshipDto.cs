namespace SFA.DAS.EmployerIncentives.Api.Models
{
    public class ApplicationApprenticeshipDto
    {
        public long ApprenticeshipId { get; set; }
        public long Uln { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CourseName { get; set; }
        public decimal ExpectedAmount { get; set; }
    }
}
using SFA.DAS.EmployerIncentives.Models;

namespace SFA.DAS.EmployerIncentives.Api.Models
{
    public class ApprenticeshipDto
    {
        public long Uln { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CourseName { get; set; }

        public static implicit operator ApprenticeshipDto(ApprenticeshipItem source)
        {
            return new ApprenticeshipDto
            {
                Uln = source.Uln,
                CourseName = source.CourseName,
                FirstName = source.FirstName,
                LastName = source.LastName
            };
        }
    }
}
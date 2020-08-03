using SFA.DAS.EmployerIncentives.Models.Commitments;

namespace SFA.DAS.EmployerIncentives.Api.Models
{
    public class EligibleApprenticeshipDto
    {
        public long ApprenticeshipId { get; set; }
        public long Uln { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CourseName { get; set; }

        public static implicit operator EligibleApprenticeshipDto(ApprenticeshipItem from)
        {
            return new EligibleApprenticeshipDto
            {
                ApprenticeshipId = from.Id,
                Uln = from.Uln,
                FirstName = from.FirstName,
                LastName = from.LastName,
                CourseName = from.CourseName
            };
        }
    }
}
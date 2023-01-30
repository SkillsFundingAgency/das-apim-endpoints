using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Funding.Models;

namespace SFA.DAS.Funding.Api.Models
{
    public class AcademicYearEarningsDto
    {
        public List<LearnerDto> Learners { get; set; }

        public static implicit operator AcademicYearEarningsDto(AcademicYearEarnings source)
        {
            return new AcademicYearEarningsDto
            {
                Learners = source.Learners.Select(x => (LearnerDto)x).ToList(),
            };
        }
    }
}

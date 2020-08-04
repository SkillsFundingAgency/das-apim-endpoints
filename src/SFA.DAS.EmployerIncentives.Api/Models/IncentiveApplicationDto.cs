using System.Collections.Generic;
using System.Linq;
using SFA.DAS.EmployerIncentives.Models;

namespace SFA.DAS.EmployerIncentives.Api.Models
{
    public class IncentiveApplicationDto
    {
        public IEnumerable<IncentiveApplicationApprenticeshipDto> Apprenticeships { get; set; }

        public static implicit operator IncentiveApplicationDto(IncentiveApplication source)
        {
            return new IncentiveApplicationDto
            {
                Apprenticeships = source.Apprenticeships.Select(x => (IncentiveApplicationApprenticeshipDto)x)
            };
        }
    }
}

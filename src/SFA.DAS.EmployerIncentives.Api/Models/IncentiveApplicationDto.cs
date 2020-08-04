using System.Collections.Generic;
using SFA.DAS.EmployerIncentives.Models;

namespace SFA.DAS.EmployerIncentives.Api.Models
{
    public class IncentiveApplicationDto
    {
        public IEnumerable<IncentiveApplicationApprenticeshipDto> Apprenticeships { get; set; }
    }
}

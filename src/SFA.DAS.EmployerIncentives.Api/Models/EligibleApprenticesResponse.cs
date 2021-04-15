using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Api.Models
{
    public class EligibleApprenticesResponse
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<EligibleApprenticeshipDto> Apprenticeships { get; set; }
        public int TotalApprenticeships { get; set; }
    }
}

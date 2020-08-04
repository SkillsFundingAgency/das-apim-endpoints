using System.Collections.Generic;

namespace SFA.DAS.EmployerIncentives.Models
{
    public class IncentiveApplication
    {
        public IEnumerable<IncentiveApplicationApprenticeship> Apprenticeships { get; set; }
    }
}

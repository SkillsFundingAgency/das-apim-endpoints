using System.Collections.Generic;

namespace SFA.DAS.EmployerIncentives.Models
{
    public class IncentiveApplication
    {
        public long AccountLegalEntityId { get; set; }
        public IEnumerable<IncentiveApplicationApprenticeship> Apprenticeships { get; set; }
    }
}

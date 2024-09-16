using System.Collections.Generic;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Models
{
    public class Dashboard
    {
        public List<AggregatedEmployerRequest> AggregatedEmployerRequests { get; set; }
        public int ExpiryAfterMonths { get; set; }
        public int RemovedAfterExpiryMonths { get; set; }
    }
}

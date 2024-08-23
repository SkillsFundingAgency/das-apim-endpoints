using System.Collections.Generic;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Api.Models
{
    public class Dashboard
    {
        public List<AggregatedEmployerRequest> AggregatedEmployerRequests { get; set; }
        public int ExpiryAfterMonths { get; set; }
    }
}

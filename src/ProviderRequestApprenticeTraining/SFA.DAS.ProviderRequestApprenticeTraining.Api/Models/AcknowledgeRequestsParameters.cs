using System.Collections.Generic;
using System;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Api.Models
{
    public class AcknowledgeRequestsParameters
    {
        public List<Guid> EmployerRequestIds { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerDemand.InnerApi.Responses
{
    public class GetEmployerDemandsOlderThan3YearsResponse
    {
        public IReadOnlyList<Guid> EmployerDemandIds { get; set; }
    }
}
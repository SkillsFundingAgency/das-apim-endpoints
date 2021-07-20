using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetCourseDemandsOlderThan3Years
{
    public class GetCourseDemandsOlderThan3YearsResult
    {
        public IReadOnlyList<Guid> EmployerDemandIds { get; set; }
    }
}


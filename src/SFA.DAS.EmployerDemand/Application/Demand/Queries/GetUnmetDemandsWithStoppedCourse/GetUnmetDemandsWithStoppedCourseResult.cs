using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetUnmetDemandsWithStoppedCourse
{
    public class GetUnmetDemandsWithStoppedCourseResult
    {
        public IEnumerable<Guid> EmployerDemandIds { get; set; }
    }
}
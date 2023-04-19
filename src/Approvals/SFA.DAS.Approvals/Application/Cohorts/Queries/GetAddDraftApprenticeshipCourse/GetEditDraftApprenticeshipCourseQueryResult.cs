using System.Collections.Generic;

namespace SFA.DAS.Approvals.Application.Cohorts.Queries.GetAddDraftApprenticeshipCourse
{
    public class GetAddDraftApprenticeshipCourseQueryResult
    {
        public string EmployerName { get; set; }
        public string ProviderName { get; set; }
        public bool IsMainProvider { get; set; }
        public IEnumerable<Standard> Standards { get; set; }

        public class Standard
        {
            public string CourseCode { get; set; }
            public string Name { get; set; }
        }
    }
}
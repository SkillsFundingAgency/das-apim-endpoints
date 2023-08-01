using System.Collections.Generic;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetEditApprenticeshipCourse
{
    public class GetEditApprenticeshipCourseQueryResult
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
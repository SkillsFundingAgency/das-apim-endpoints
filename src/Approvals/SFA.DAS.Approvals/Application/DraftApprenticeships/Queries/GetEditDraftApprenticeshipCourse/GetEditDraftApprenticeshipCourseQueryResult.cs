using System.Collections.Generic;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeshipCourse
{
    public class GetEditDraftApprenticeshipCourseQueryResult
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
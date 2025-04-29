using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetProvidersListResponse
    {
        public IEnumerable<GetProvidersListItem> Providers { get ; set ; }
        public int TotalResults { get; set; }
    }

    public class GetProvidersListFromCourseIdResponse
    {
        public IEnumerable<GetProvidersListItem> Providers { get; set; }
        public int LarsCode { get; set; }
        public string CourseTitle { get; set; }
        public int Level { get; set; }
    }
}
using System.Collections.Generic;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCoursesList
{
    public class GetTrainingCoursesListResult
    {
        public IEnumerable<GetStandardsListItem> Courses { get; set; }
        public IEnumerable<GetRoutesListItem> Sectors { get ; set ; }
        public IEnumerable<GetLevelsListItem> Levels { get ; set ; }
        public int Total { get ; set ; }
        public int TotalFiltered { get ; set ; }
        public OrderBy OrderBy { get; set; }
        public int ShortlistItemCount { get ; set ; }
    }
}

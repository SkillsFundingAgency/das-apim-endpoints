using System.Collections.Generic;
using SFA.DAS.FindApprenticeshipTraining.Application;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetTrainingCoursesListResponse
    {
        public IEnumerable<GetTrainingCourseListItem> TrainingCourses { get; set; }
        public int Total { get ; set ; }
        public int TotalFiltered { get ; set ; }
        public IEnumerable<GetTrainingSectorsListItem> Sectors { get ; set ; }
        public IEnumerable<GetTrainingLevelsListItem> Levels { get ; set ; }
        public int ShortlistItemCount { get ; set ; }
    }
}

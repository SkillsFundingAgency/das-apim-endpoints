using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourse
{
    public class GetTrainingCourseResult
    {
        public GetStandardsListItem Course { get ; set ; }
        public int ProvidersCount { get; set; }
    }
}
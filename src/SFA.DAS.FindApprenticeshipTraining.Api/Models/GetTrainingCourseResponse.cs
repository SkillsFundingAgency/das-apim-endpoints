namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetTrainingCourseResponse
    {
        public GetTrainingCourseListItem TrainingCourse { get; set; }
        public GetTrainingCourseProviderCountResponse ProvidersCount { get; set; }
        public string LevelEquivalent { get; set; }
    }
}
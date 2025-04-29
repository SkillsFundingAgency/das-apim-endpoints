namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetTrainingCourseProviderResponse
    {
        public GetTrainingCourseListItem TrainingCourse { get ; set ; }
        public GetProviderCourseItem TrainingCourseProvider { get; set; }
        public GetTrainingAdditionalCourseItem AdditionalCourses { get; set; }
        public GetTrainingCourseProviderCountResponse ProvidersCount { get ; set ; }
        public GetLocationSearchResponseItem Location { get ; set ; }
        public int ShortlistItemCount { get ; set ; }
    }
}
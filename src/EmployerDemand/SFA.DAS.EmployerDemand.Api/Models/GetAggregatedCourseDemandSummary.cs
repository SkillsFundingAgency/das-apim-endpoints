namespace SFA.DAS.EmployerDemand.Api.Models
{
    public class GetAggregatedCourseDemandSummary
    {
        public GetCourseListItem TrainingCourse { get; set; }
        public int EmployersCount { get; set; }
        public int ApprenticesCount { get; set; }

        public static implicit operator GetAggregatedCourseDemandSummary(
            InnerApi.Responses.GetAggreatedCourseDemandSummaryResponse source)
        {
            return new GetAggregatedCourseDemandSummary
            {
                EmployersCount = source.EmployersCount,
                ApprenticesCount = source.ApprenticesCount,
                TrainingCourse = new GetCourseListItem
                {
                    Id = source.CourseId,
                    Level = source.CourseLevel,
                    Route = source.CourseRoute,
                    Title = source.CourseTitle
                }
            };
        }
    }
}

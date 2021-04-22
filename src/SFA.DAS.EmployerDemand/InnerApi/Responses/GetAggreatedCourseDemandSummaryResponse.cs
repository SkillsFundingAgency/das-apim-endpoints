namespace SFA.DAS.EmployerDemand.InnerApi.Responses
{
    public class GetAggreatedCourseDemandSummaryResponse
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public int CourseLevel { get; set; }
        public string CourseRoute { get; set; }
        public int EmployersCount { get; set; }
        public int ApprenticesCount { get; set; }
    }
}
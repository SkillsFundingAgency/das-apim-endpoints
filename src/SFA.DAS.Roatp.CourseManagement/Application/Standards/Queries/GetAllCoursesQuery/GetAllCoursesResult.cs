namespace SFA.DAS.Roatp.CourseManagement.Application.Standards.Queries.GetAllCoursesQuery
{
    public class GetAllCoursesResult
    {
        public int ProviderCourseId { get; set; }
        public string CourseName { get; set; }
        public int Level { get; set; }
        public bool IsImported { get; set; }
        public int LarsCode { get; set; }
    }
}

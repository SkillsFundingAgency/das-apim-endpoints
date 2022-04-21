namespace SFA.DAS.Roatp.CourseManagement.Queries.GetCourseQuery
{
    public class GetAllCoursesResult
    {
        public int ProviderCourseId { get; set; }
        public string CourseName { get; set; }
        public int Level { get; set; }
        public bool IsImported { get; set; } = false;
    }
}

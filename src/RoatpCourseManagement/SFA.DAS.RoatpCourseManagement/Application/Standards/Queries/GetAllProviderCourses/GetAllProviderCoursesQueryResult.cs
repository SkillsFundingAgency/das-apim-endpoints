namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAllProviderCourses
{
    public class GetAllProviderCoursesQueryResult
    {
        public int ProviderCourseId { get; set; }
        public string CourseName { get; set; }
        public int Level { get; set; }
        public bool IsImported { get; set; }
        public int LarsCode { get; set; }
        public string Version { get; set; }
        public string ApprovalBody { get; set; }
        public bool? IsApprovedByRegulator { get; set; }
    }
}

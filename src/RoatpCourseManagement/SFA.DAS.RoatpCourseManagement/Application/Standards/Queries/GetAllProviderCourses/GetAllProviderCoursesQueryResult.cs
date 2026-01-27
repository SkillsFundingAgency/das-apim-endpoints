namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAllProviderCourses
{
    public class GetAllProviderCoursesQueryResult
    {
        public int ProviderCourseId { get; set; }
        public string CourseName { get; set; }
        public int Level { get; set; }
        public string LarsCode { get; set; }
        public string ApprovalBody { get; set; }
        public bool? IsApprovedByRegulator { get; set; }
        public bool IsRegulatedForProvider { get; set; }
        public bool HasLocations { get; set; }
        public bool HasOnlineDeliveryOption { get; set; }
    }
}

using SFA.DAS.AdminRoatp.InnerApi.Models;

namespace SFA.DAS.AdminRoatp.InnerApi.Responses;

public class GetAllRestrictedCoursesResponse
{
    public List<RestrictedCourseModel> Courses { get; set; } = new List<RestrictedCourseModel>();
}

using System.Collections.Generic;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Responses;

public class GetAllRestrictedCoursesResponse
{
    public List<RestrictedCourseModel> Courses { get; set; } = new List<RestrictedCourseModel>();
}

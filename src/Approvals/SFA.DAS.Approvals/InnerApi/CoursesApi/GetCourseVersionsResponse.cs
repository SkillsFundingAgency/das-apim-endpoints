using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.CoursesApi;

public class GetCourseVersionsResponse
{
    public List<GetCourseLookupResponse> Courses { get; set; }
}

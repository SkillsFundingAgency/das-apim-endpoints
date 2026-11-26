using System.Collections.Generic;

namespace SFA.DAS.Approvals.Api.Models;
public class GetCoursesListResponse
{
    public IEnumerable<GetCourseResponse> Courses { get; set; }
}
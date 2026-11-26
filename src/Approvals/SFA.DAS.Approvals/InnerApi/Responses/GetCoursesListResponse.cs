using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Responses;
public class GetCoursesListResponse
{
    public IEnumerable<GetCoursesListItem> Courses { get; set; }
}
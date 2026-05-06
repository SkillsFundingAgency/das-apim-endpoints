using System.Collections.Generic;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Application.TrainingCourses.Queries;

public class GetCoursesResult
{
    public IEnumerable<GetCoursesListItem> Courses { get; set; }
}
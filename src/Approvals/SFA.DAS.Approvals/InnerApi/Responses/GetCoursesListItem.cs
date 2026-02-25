using System;
using SFA.DAS.Approvals.Types;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.Approvals.InnerApi.Responses;

public class GetCoursesListItem : CourseApiResponseBase
{
    public string LarsCode { get; set; }
    public string Title { get; set; }
    public int Level { get; set; }
    public LearningType LearningType { get; set; }
}
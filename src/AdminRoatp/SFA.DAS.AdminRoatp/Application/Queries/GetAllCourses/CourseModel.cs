using SFA.DAS.Common.Domain.Types;
using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetAllCourses;

public class CourseModel
{
    public string LarsCode { get; set; } = string.Empty;
    public CourseType CourseType { get; set; }
    public LearningType LearningType { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Level { get; set; }
}

using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.AdminRoatp.InnerApi.Models;

public class ProviderCourseTypeModel
{
    public int CourseTypeId { get; set; }
    public CourseType CourseType { get; set; }
    public bool IsRestricted { get; set; }
    public int? RestrictedCount { get; set; }
    public int? AllowedCount { get; set; }
}

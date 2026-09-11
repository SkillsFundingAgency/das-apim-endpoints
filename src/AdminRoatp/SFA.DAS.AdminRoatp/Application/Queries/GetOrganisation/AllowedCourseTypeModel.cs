using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetOrganisation;

public class AllowedCourseTypeModel
{
    public CourseType CourseType { get; set; }
    public bool? IsRestricted { get; set; }
    public int? RestrictedCount { get; set; }
    public int? AllowedCount { get; set; }

    public static implicit operator AllowedCourseTypeModel(ProviderCourseTypeModel source) => new()
    {
        CourseType = source.CourseType,
        IsRestricted = source.IsRestricted,
        RestrictedCount = source.RestrictedCount,
        AllowedCount = source.AllowedCount
    };
}

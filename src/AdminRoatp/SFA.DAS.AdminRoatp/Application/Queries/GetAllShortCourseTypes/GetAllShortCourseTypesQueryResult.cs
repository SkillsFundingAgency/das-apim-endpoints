using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetAllShortCourseTypes;
public class GetAllShortCourseTypesQueryResult
{
    public IEnumerable<CourseTypeSummary> CourseTypes { get; set; } = Enumerable.Empty<CourseTypeSummary>();
}
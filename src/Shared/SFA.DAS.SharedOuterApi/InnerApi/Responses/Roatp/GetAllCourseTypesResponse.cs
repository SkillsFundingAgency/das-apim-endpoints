using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
public class GetAllCourseTypesResponse
{
    public IEnumerable<CourseTypeSummary> CourseTypes { get; set; } = Enumerable.Empty<CourseTypeSummary>();
}

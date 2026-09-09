using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;

namespace SFA.DAS.EmployerFinance.InnerApi.Requests;

public class GetCoursesRequest : IGetApiRequest
{
    public string GetUrl => $"api/courses/search?filter=Active&orderby={CoursesOrderBy.Score}";
}
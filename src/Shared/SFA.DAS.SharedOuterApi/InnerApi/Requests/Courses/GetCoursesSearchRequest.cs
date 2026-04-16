using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Courses;

public class GetCoursesSearchRequest : IGetApiRequest
{
    public string GetUrl => "api/courses/search?filter=ActiveAvailable&courseType=ShortCourse";
}

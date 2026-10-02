using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;

public class GetOldCoursesRequest : IGetApiRequest
{
    public string GetUrl => "api/Courses/search?filter=ClosedToNewStarts";
}
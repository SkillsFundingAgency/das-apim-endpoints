using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses
{
    public class GetCourseLevelsListRequest : IGetApiRequest
    {
        public string GetUrl => "api/courses/levels";
    }
}

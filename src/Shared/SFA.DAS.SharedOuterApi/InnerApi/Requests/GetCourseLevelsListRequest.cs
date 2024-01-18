using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetCourseLevelsListRequest : IGetApiRequest
    {
        public string GetUrl => "api/courses/levels";
    }
}

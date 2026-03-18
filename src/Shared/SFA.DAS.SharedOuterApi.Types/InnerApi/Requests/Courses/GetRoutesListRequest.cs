using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses
{
    public class GetRoutesListRequest : IGetApiRequest
    {
        public string GetUrl => "api/courses/routes";
    }
}
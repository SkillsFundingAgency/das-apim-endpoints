using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses
{
    public class GetAllStandardsRequest : IGetApiRequest
    {
        public string GetUrl => "api/courses/standards?filter=None";
    }
}
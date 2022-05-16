using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetAllFrameworksRequest : IGetApiRequest
    {
        public string GetUrl => "api/courses/frameworks";
    }
}
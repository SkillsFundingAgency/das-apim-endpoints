using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses
{
    public class GetFrameworkRequest : IGetApiRequest
    {
        public GetFrameworkRequest(string code)
        {
            FrameworkCode = code;
        }

        public string FrameworkCode { get; }
        public string GetUrl => $"api/courses/frameworks/{FrameworkCode}";
    }
}
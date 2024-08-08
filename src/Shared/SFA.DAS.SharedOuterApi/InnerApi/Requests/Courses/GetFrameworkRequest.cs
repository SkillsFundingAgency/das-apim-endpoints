using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
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
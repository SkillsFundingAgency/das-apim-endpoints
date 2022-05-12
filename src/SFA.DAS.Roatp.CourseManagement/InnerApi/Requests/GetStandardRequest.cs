using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Roatp.CourseManagement.InnerApi.Requests
{
    public class GetStandardRequest : IGetApiRequest
    {
        public string GetUrl => $"api/courses/Standards/{StandardCode}";
        public int StandardCode { get; }

        public GetStandardRequest(int standardCode)
        {
            StandardCode = standardCode;
        }
    }
}

using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Roatp.CourseManagement.InnerApi.Requests
{
    public class GetStandardRequest : IGetApiRequest
    {
        public string GetUrl => $"api/courses/Standards/{LarsCode}";
        public int LarsCode { get; }

        public GetStandardRequest(int larsCode)
        {
            LarsCode = larsCode;
        }
    }
}

using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests
{
    public class GetStandardRequest : IGetApiRequest
    {
        public string GetUrl => $"api/courses/Standards/{LarsCode}";
        public string LarsCode { get; }

        public GetStandardRequest(string larsCode)
        {
            LarsCode = larsCode;
        }
    }
}

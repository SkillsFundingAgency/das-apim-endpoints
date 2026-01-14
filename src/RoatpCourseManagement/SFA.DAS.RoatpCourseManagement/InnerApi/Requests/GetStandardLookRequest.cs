using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests
{
    public class GetStandardLookRequest : IGetApiRequest
    {
        public string GetUrl => $"standards/{LarsCode}";
        public string LarsCode { get; }

        public GetStandardLookRequest(string larsCode)
        {
            LarsCode = larsCode;
        }
    }
}

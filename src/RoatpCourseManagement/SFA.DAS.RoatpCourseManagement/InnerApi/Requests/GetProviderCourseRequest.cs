using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests
{

    public class GetProviderCourseRequest : IGetApiRequest
    {
        public string GetUrl => $"providers/{Ukprn}/courses/{LarsCode}";
        public int Ukprn { get; }
        public int LarsCode { get; }

        public GetProviderCourseRequest(int ukprn,int larsCode)
        {
            Ukprn = ukprn;
            LarsCode = larsCode;
        }
    }
}

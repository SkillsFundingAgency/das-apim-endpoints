using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests
{

    public class GetProviderCourseRequest : IGetApiRequest
    {
        public string GetUrl => $"providers/{Ukprn}/courses/{LarsCode}";
        public int Ukprn { get; }
        public string LarsCode { get; }

        public GetProviderCourseRequest(int ukprn, string larsCode)
        {
            Ukprn = ukprn;
            LarsCode = larsCode;
        }
    }
}

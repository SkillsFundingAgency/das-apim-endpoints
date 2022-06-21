using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests
{
    public class GetProviderCourseLocationsRequest : IGetApiRequest
    {
        public string GetUrl => $"providers/{Ukprn}/courses/{LarsCode}/locations";
        public int Ukprn { get; }
        public int LarsCode { get; }

        public GetProviderCourseLocationsRequest(int ukprn, int larsCode)
        {
            Ukprn = ukprn;
            LarsCode = larsCode;
        }
    }
}


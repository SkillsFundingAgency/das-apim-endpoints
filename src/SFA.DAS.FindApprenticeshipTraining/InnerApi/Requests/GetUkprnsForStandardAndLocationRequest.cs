using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetUkprnsForStandardAndLocationRequest : IGetApiRequest
    {
        private readonly int _standardId;
        private readonly double _lat;
        private readonly double _lon;

        public GetUkprnsForStandardAndLocationRequest(int standardId, double lat, double lon)
        {
            _standardId = standardId;
            _lat = lat;
            _lon = lon;
        }

        public string GetUrl => $"/api/courses/{_standardId}/ukprns?lat={_lat}&lon={_lon}";
    }
}
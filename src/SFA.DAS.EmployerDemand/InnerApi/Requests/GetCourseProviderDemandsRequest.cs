using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.InnerApi.Requests
{
    public class GetCourseProviderDemandsRequest : IGetApiRequest
    {
        private readonly int _ukprn;
        private readonly int _courseId;
        private readonly double? _lat;
        private readonly double? _lon;
        private readonly int? _radius;

        public GetCourseProviderDemandsRequest(int ukprn, int courseId, double? lat = null, double? lon = null, int? radius = null)
        {
            _ukprn = ukprn;
            _courseId = courseId;
            _lat = lat;
            _lon = lon;
            _radius = radius;
        }

        public string GetUrl => $"api/Demand/providers/{_ukprn}/courses/{_courseId}?lat={_lat}&lon={_lon}&radius={_radius}";
    }
}
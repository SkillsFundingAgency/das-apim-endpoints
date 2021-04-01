using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.InnerApi.Requests
{
    public class GetAggregatedCourseDemandListRequest : IGetApiRequest
    {
        public string GetUrl => $"api/demand/aggregated/providers/{Ukprn}?courseId={CourseId}&lat={Lat}&lon={Lon}&radius={Radius}";
        public int Ukprn { get; }
        public int? CourseId { get; }
        public double? Lat { get; }
        public double? Lon { get; }
        public int? Radius { get; }

        public GetAggregatedCourseDemandListRequest(int ukprn, int? courseId = null, double? lat = null, double? lon = null, int? radius = null)
        {
            Ukprn = ukprn;
            CourseId = courseId;
            Lat = lat;
            Lon = lon;
            Radius = radius;
        }
    }
}
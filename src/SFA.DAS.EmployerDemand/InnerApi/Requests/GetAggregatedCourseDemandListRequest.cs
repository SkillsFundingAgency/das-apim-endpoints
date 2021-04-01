using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.InnerApi.Requests
{
    public class GetAggregatedCourseDemandListRequest : IGetApiRequest
    {
        public string GetUrl => $"api/demand/aggregated/providers/{Ukprn}?courseId={CourseId}&lat={Lat}&lon={Lon}";
        public int Ukprn { get; }
        public int? CourseId { get; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }

        public GetAggregatedCourseDemandListRequest(int ukprn, int? courseId = null, double? lat = null, double? lon = null)
        {
            Ukprn = ukprn;
            CourseId = courseId;
        }
    }
}
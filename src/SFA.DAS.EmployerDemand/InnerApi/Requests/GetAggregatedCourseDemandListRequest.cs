using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.InnerApi.Requests
{
    public class GetAggregatedCourseDemandListRequest : IGetApiRequest
    {
        public string GetUrl => $"api/demand/aggregated/providers/{Ukprn}?courseId={CourseId}";
        public int Ukprn { get; }
        public int CourseId { get; }

        public GetAggregatedCourseDemandListRequest(int ukprn, int courseId)
        {
            Ukprn = ukprn;
            CourseId = courseId;
        }
    }
}
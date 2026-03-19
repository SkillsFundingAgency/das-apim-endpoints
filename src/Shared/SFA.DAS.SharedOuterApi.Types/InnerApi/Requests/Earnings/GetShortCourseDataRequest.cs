using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Earnings
{
    public class GetShortCourseDataRequest : IGetApiRequest
    {
        public string LearningKey { get; }
        public long Ukprn { get; }
        public string GetUrl => $"{LearningKey}/shortCourses?ukprn={Ukprn}";

        public GetShortCourseDataRequest(long ukprn, Guid learningKey)
        {
            Ukprn = ukprn;
            LearningKey = learningKey.ToString();
        }
    }
}

using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.Requests.EarningsInner;

public class GetShortCourseEarningsRequest : IGetApiRequest
{
    public string LearningKey { get; }
    public long Ukprn { get; }
    public string GetUrl => $"{LearningKey}/shortCourses?ukprn={Ukprn}";

    public GetShortCourseEarningsRequest(long ukprn, Guid learningKey)
    {
        Ukprn = ukprn;
        LearningKey = learningKey.ToString();
    }
}
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.LearnerData.Requests.EarningsInner;

public class GetFm99ShortCourseDataRequest : IGetApiRequest
{
    public string LearningKey { get; }
    public long Ukprn { get; }
    public string GetUrl => $"fm99/{LearningKey}/shortCourses?ukprn={Ukprn}";

    public GetFm99ShortCourseDataRequest(long ukprn, Guid learningKey)
    {
        Ukprn = ukprn;
        LearningKey = learningKey.ToString();
    }
}

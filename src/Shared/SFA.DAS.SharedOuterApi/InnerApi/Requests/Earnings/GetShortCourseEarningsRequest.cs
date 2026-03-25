using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Earnings;

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
using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Earnings
{
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
}

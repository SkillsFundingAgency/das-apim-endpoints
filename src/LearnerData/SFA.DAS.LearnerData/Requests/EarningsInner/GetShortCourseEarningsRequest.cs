using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.LearnerData.Requests.EarningsInner;

public class GetShortCourseEarningsRequest : IGetApiRequest
{
    public string LearningKey { get; }
    public string EpisodeKey { get; }
    public string GetUrl => $"{LearningKey}/shortCourses/{EpisodeKey}";

    public GetShortCourseEarningsRequest(Guid learningKey, Guid episodeKey)
    {
        LearningKey = learningKey.ToString();
        EpisodeKey = episodeKey.ToString();
    }
}

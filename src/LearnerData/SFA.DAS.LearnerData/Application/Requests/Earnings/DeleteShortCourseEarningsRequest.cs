using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.LearnerData.Application.Requests.Earnings;

public class DeleteShortCourseEarningsRequest(Guid learningKey, Guid episodeKey) : IDeleteApiRequest
{
    public Guid LearningKey { get; } = learningKey;
    public Guid EpisodeKey { get; } = episodeKey;
    public Guid LearnerKey { get; private set; }
    public string LearnerRef { get; private set; } = string.Empty;
    public string DeleteUrl => $"{LearningKey}/shortCourses/{EpisodeKey}?learnerKey={LearnerKey}&learnerRef={Uri.EscapeDataString(LearnerRef)}";

    public DeleteShortCourseEarningsRequest(Guid learningKey, Guid episodeKey, Guid learnerKey, string learnerRef) : this(learningKey, episodeKey)
    {
        LearnerKey = learnerKey;
        LearnerRef = learnerRef;
    }
}

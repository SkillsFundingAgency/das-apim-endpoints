using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.LearnerData.Application.Requests.Earnings;

public class DeleteShortCourseEarningsRequest(Guid learningKey, Guid episodeKey) : IDeleteApiRequest
{
    public Guid LearningKey { get; } = learningKey;
    public Guid EpisodeKey { get; } = episodeKey;
    public string DeleteUrl => $"{LearningKey}/shortCourses/{EpisodeKey}";
}

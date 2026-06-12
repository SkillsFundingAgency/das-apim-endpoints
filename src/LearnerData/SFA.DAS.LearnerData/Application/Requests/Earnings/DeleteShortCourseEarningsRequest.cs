using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.LearnerData.Application.Requests.Earnings;

public class DeleteShortCourseEarningsRequest(Guid learnerKey, Guid episodeKey) : IDeleteApiRequest
{
    public Guid LearnerKey { get; } = learnerKey;
    public Guid EpisodeKey { get; } = episodeKey;
    public string DeleteUrl => $"{LearnerKey}/shortCourses/{EpisodeKey}";
}

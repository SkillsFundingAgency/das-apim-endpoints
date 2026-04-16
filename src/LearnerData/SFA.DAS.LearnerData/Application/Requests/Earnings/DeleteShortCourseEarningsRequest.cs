using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.Application.Requests.Earnings;

public class DeleteShortCourseEarningsRequest(Guid learningKey) : IDeleteApiRequest
{
    public Guid LearningKey { get; } = learningKey;
    public string DeleteUrl => $"{LearningKey}/shortCourses";
}

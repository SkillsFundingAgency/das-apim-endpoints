using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.Application.Requests.Learning;

public class DeleteShortCourseApiDeleteRequest : IDeleteApiRequest
{
    public DeleteShortCourseApiDeleteRequest(long ukprn, Guid learningKey)
    {
        Ukprn = ukprn;
        LearningKey = learningKey;
    }

    public long Ukprn { get; }
    public Guid LearningKey { get; }
    public string DeleteUrl => $"{Ukprn}/shortCourses/{LearningKey}";
}

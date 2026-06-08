namespace SFA.DAS.LearnerData.Responses.LearningInner;

public class DeleteShortCourseResponse : IShortCourseLearningPaymentEventBuildContext
{
    public Guid RemovedEpisodeKey { get; set; }
    public Guid LearningKey { get; set; }
    public Guid LearnerKey { get; set; }
    public LearningInnerShortCourseLearner Learner { get; set; } = null!;
    public LearningInnerShortCourseEpisode[] Episodes { get; set; } = [];
}

namespace SFA.DAS.LearnerData.Responses.LearningInner;

public class DeleteShortCourseResponse : IShortCourseLearningPaymentEventBuildContext
{
    public Guid LearningKey { get; set; }
    public DateTime? CompletionDate { get; set; }
    public LearningInnerShortCourseLearner Learner { get; set; } = null!;
    public LearningInnerShortCourseEpisode[] Episodes { get; set; } = [];
}

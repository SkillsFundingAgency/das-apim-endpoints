namespace SFA.DAS.LearnerData.Responses.LearningInner;

#pragma warning disable CS8618
public class CreateShortCoursePostResponse : IShortCourseLearningPaymentEventBuildContext
{
    public Guid LearningKey { get; set; }
    public Guid EpisodeKey { get; set; }
    public bool IsReinstated { get; set; }
    public bool IsIgnored { get; set; }
    public Guid LearnerKey { get; set; }
    public LearningInnerShortCourseLearner Learner { get; set; }
    public LearningInnerShortCourseEpisode[] Episodes { get; set; }
}
#pragma warning restore CS8618

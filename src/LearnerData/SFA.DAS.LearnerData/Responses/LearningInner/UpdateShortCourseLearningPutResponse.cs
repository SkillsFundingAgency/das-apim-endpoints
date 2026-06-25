namespace SFA.DAS.LearnerData.Responses.LearningInner;

public class UpdateShortCourseLearningResponse
{
    public List<UpdateShortCourseLearningPutResponse> Results { get; set; } = [];
}

public class UpdateShortCourseLearningPutResponse : IShortCourseLearningPaymentEventBuildContext
{
    public Guid UpdatedEpisodeKey { get; set; }
    public Guid LearningKey { get; set; }
    public Guid LearnerKey { get; set; }
    public string CourseCode { get; set; } = "";
    public string[] Changes { get; set; } = [];
    public LearningInnerShortCourseLearner Learner { get; set; } = null!;
    public LearningInnerShortCourseEpisode[] Episodes { get; set; } = [];
    public bool IsNewLearning { get; set; }
    public bool IsIgnored { get; set; }
    public bool IsRemoved { get; set; }
}

public static class UpdateShortCourseLearningPutResponseExtensions
{
    public static ShortCourseUpdateChanges[] GetChangesEnums(this UpdateShortCourseLearningPutResponse response)
    {
        return response.Changes
            .Select(c => Enum.TryParse<ShortCourseUpdateChanges>(c, out var result)
                ? result
                : throw new InvalidOperationException($"Unknown change type '{c}'"))
            .ToArray();
    }
}

public enum ShortCourseUpdateChanges
{
    WithdrawalDate = 0,
    Milestone = 1,
    CompletionDate = 2,
    LearnerRef = 3,
    Reinstated = 4,
    StartDate = 5,
    ExpectedEndDate = 6
}
#pragma warning restore CS8618
namespace SFA.DAS.LearnerData.Responses.LearningInner;

public class UpdateShortCourseLearningPutResponse : IShortCourseLearningPaymentEventBuildContext
{
    public Guid LearningKey { get; set; }
    public string[] Changes { get; set; } = [];
    public DateTime? CompletionDate { get; set; }
    public LearningInnerShortCourseLearner Learner { get; set; } = null!;
    public LearningInnerShortCourseEpisode[] Episodes { get; set; } = [];
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
    LearnerRef = 3

}
#pragma warning restore CS8618
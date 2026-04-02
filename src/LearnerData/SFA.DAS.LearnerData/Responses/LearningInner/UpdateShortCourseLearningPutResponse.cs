namespace SFA.DAS.LearnerData.Responses.LearningInner;

public class UpdateShortCourseLearningPutResponse
{
    public Guid LearningKey { get; set; }
    public string[] Changes { get; set; } = [];
    public DateTime? CompletionDate { get; set; }
    public UpdateShortCourseResultLearner Learner { get; set; } = null!;
    public UpdateShortCourseResultEpisode[] Episodes { get; set; } = [];
}

public class UpdateShortCourseResultLearner
{
    public string Uln { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
}

public class UpdateShortCourseResultEpisode
{
    public long Ukprn { get; set; }
    public long EmployerAccountId { get; set; }
    public string CourseCode { get; set; } = null!;
    public string CourseType { get; set; } = null!;
    public string LearningType { get; set; }
    public DateTime StartDate { get; set; }
    public int AgeAtStart { get; set; }
    public DateTime PlannedEndDate { get; set; }
    public DateTime? WithdrawalDate { get; set; }
    public bool IsApproved { get; set; }
    public decimal Price { get; set; }
    public string LearnerRef { get; set; } = null!;
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
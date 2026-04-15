namespace SFA.DAS.LearnerData.Responses.LearningInner;

#pragma warning disable CS8618
public interface IShortCourseLearningPaymentEventBuildContext
{
    public Guid LearningKey { get; set; }
    public Guid LearnerKey { get; set; }
    public DateTime? CompletionDate { get; set; }
    public LearningInnerShortCourseLearner Learner { get; set; }
    public LearningInnerShortCourseEpisode[] Episodes { get; set; }
}

public class LearningInnerShortCourseLearner
{
    public string Uln { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
}

public class LearningInnerShortCourseEpisode
{
    public long Ukprn { get; set; }
    public long EmployerAccountId { get; set; }
    public string EmployerType { get; set; }
    public string CourseCode { get; set; }
    public string CourseType { get; set; }
    public string LearningType { get; set; }
    public DateTime StartDate { get; set; }
    public int AgeAtStart { get; set; }
    public DateTime PlannedEndDate { get; set; }
    public DateTime? WithdrawalDate { get; set; }
    public bool IsApproved { get; set; }
    public decimal Price { get; set; }
    public string LearnerRef { get; set; }
    public long ApprovalsApprenticeshipId { get; set; }
}
#pragma warning restore CS8618
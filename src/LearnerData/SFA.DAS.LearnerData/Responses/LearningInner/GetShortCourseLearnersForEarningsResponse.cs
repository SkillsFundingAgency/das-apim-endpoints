namespace SFA.DAS.LearnerData.Responses.LearningInner.GetShortCourseLearnersForEarningsResponse;

public class Learning
{
    public Guid LearningKey { get; set; }
    public Learner Learner { get; set; }
    public List<Episode> Episodes { get; set; }
}

public class Learner
{
    public string Uln { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
}

public class Episode
{
    public string CourseCode { get; set; }
    public decimal Price { get; set; }
    public bool IsApproved { get; set; }
    public string LearnerRef { get; set; }
}

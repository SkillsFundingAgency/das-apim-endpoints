namespace SFA.DAS.LearnerData.Application.GetShortCourseEarnings;

public class GetShortCourseEarningsQueryResult
{
    public List<ShortCourseEarningsLearner> Learners { get; set; }
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}

#pragma warning disable CS8618
public class ShortCourseEarningsLearner
{
    public string LearningKey { get; set; }
    public string LearnerRef { get; set; } = "";
    public List<ShortCourseEarningsCourse> Courses { get; set; }
}

public class ShortCourseEarningsCourse
{
    public int AimSequenceNumber { get; set; }
    public string FundingLineType { get; set; }
    public decimal CoursePrice { get; set; }
    public bool Approved { get; set; }
    public List<ShortCourseEarningsEarning> Earnings { get; set; }
}

public class ShortCourseEarningsEarning
{
    public int CollectionYear { get; set; }
    public int CollectionPeriod { get; set; }
    public string Milestone { get; set; }
    public decimal Amount { get; set; }
}
#pragma warning restore CS8618

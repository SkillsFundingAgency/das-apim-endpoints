using SFA.DAS.LearnerData.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning.GetShortCourseLearnersForEarningsResponse;

namespace SFA.DAS.LearnerData.Application.GetShortCourseEarnings;

public class GetShortCourseEarningsResult : PagedQueryResult<ShortCourseEarnings>
{

}

public class GetPagedLearnersFromLearningInner : PagedQueryResult<Learning>
{

}

#pragma warning disable CS8618
public class ShortCourseEarnings
{
    public string LearningKey { get; set; }
    public string LearnerRef { get; set; }
    public List<Course> Courses { get; set; }
}

public class Course
{
    public int CoursePrice { get; set; }
    public bool Approved { get; set; }
    public Earning[] Earnings { get; set; }
}

public class Earning
{
    public int CollectionYear { get; set; }
    public int CollectionPeriod { get; set; }
    public string Milestone { get; set; }
    public int Amount { get; set; }
}
#pragma warning restore CS8618
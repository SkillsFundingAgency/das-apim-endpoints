using SFA.DAS.LearnerData.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning.GetShortCourseLearnersForEarningsResponse;

namespace SFA.DAS.LearnerData.Application.GetShortCourseEarnings;

public class GetShortCourseEarningsResult : PagedQueryResult<ShortCourseActiveEarnings>
{

}

public class GetPagedLearnersFromLearningInner : PagedQueryResult<Learning>
{

}

#pragma warning disable CS8618
/// <summary>
/// This is combined data from the Learning and Earnings API for approved short courses
/// that are active during the period specified in the query
/// </summary>
public class ShortCourseActiveEarnings
{
    public string LearningKey { get; set; }
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
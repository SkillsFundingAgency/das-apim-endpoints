using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning.GetShortCourseLearnersForEarningsResponse;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests.Models;

internal class ShortCourseTestData
{
    internal string Ukprn { get; set; }
    internal List<Learning> ShortCourseLearnings { get; set; }
    internal Dictionary<Guid, GetFm99ShortCourseDataResponse> ShortCourseEarnings { get; set; }

    internal ShortCourseTestData(string ukprn)
    {
        Ukprn = ukprn;
        ShortCourseLearnings = new List<Learning>();
        ShortCourseEarnings = new Dictionary<Guid, GetFm99ShortCourseDataResponse>();
    }

    /// <param name="shortCourseEarnings">Earnings must appear in the same order as learnings</param>
    internal ShortCourseTestData(string ukprn, List<Learning> shortCourseLearnings, List<GetFm99ShortCourseDataResponse> shortCourseEarnings)
    {
        Ukprn = ukprn;
        ShortCourseLearnings = shortCourseLearnings;

        ShortCourseEarnings = new Dictionary<Guid, GetFm99ShortCourseDataResponse>();
        for (var i = 0; i < shortCourseLearnings.Count; i++)
        {
            ShortCourseEarnings.Add(shortCourseLearnings[i].LearningKey, shortCourseEarnings[i]);
        }
    }
}

public class ShortCoursePaginationExpectations
{
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int NumberOfRecordsInPage { get; set; }
}

public class ShortCourseDetailsExpectations
{
    public decimal Price { get; set; }
    public bool IsApproved { get; set; }
    public int CollectionYear1 { get; set; }
    public byte Period1 { get; set; }
    public decimal Amount1 { get; set; }
    public string Type1 { get; set; }
    public int CollectionYear2 { get; set; }
    public byte Period2 { get; set; }
    public decimal Amount2 { get; set; }
    public string Type2 { get; set; }
}
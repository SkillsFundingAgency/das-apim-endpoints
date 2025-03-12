namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseByLarsCode;

public sealed class GetCourseByLarsCodeQueryResult
{
    public string StandardUId { get; set; }
    public string IFateReferenceNumber { get; set; }
    public int LarsCode { get; set; }
    public int ProvidersCount { get; set; }
    public int TotalProvidersCount { get; set; }
    public string Title { get; set; }
    public int Level { get; set; }
    public string Version { get; set; }
    public string OverviewOfRole { get; set; }
    public string Route { get; set; }
    public int RouteCode { get; set; }
    public int MaxFunding { get; set; }
    public int TypicalDuration { get; set; }
    public string TypicalJobTitles { get; set; }
    public string StandardPageUrl { get; set; }
    public string[] Skills { get; set; }
    public string[] Knowledge { get; set; }
    public string[] Behaviours { get; set; }
}

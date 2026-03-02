using System.Collections.Generic;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourses;

public sealed class GetCoursesQueryResult
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public List<StandardModel> Standards { get; set; } = new List<StandardModel>();
}

public sealed class StandardModel
{
    public int Ordering { get; set; }
    public string StandardUId { get; set; }
    public string IfateReferenceNumber { get; set; }
    public string LarsCode { get; set; }
    public float? SearchScore { get; set; }
    public int ProvidersCount { get; set; } = 0;
    public int TotalProvidersCount { get; set; } = 0;
    public string Title { get; set; }
    public int Level { get; set; }
    public string OverviewOfRole { get; set; }
    public string Keywords { get; set; }
    public string Route { get; set; }
    public int RouteCode { get; set; }
    public int MaxFunding { get; set; }
    public int TypicalDuration { get; set; }
    public string ApprenticeshipType { get; set; }

    public static StandardModel CreateFrom(
        GetStandardsListItem source,
        int order,
        int providerCount,
        int totalProvidersCount
    )
        => new StandardModel
        {
            Ordering = order,
            StandardUId = source.StandardUId,
            IfateReferenceNumber = source.IfateReferenceNumber,
            LarsCode = source.LarsCode.ToString(),
            SearchScore = source.SearchScore,
            ProvidersCount = providerCount,
            TotalProvidersCount = totalProvidersCount,
            Title = source.Title,
            Level = source.Level,
            OverviewOfRole = source.OverviewOfRole,
            Keywords = source.Keywords,
            Route = source.Route,
            RouteCode = source.RouteCode,
            MaxFunding = source.MaxFunding,
            TypicalDuration = source.TypicalDuration,
            ApprenticeshipType = source.ApprenticeshipType.ToString()
        };
}
using System.Collections.Generic;
using MediatR;
using SFA.DAS.SharedOuterApi.Common;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourses;

#nullable enable

public sealed class GetCoursesQuery : IRequest<GetCoursesQueryResult>
{
    public string? Keyword { get; set; }

    public List<int> RouteIds { get; set; } = [];

    public List<int> Levels { get; set; } = [];
    public List<ApprenticeshipType> LearningTypes { get; set; } = [];
    public int? Distance { get; set; }

    public string? Location { get; set; }

    public CoursesOrderBy OrderBy { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}
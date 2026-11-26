using System.Collections.Generic;
using MediatR;
using SFA.DAS.SharedOuterApi.Types.Constants;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourses;

#nullable enable

public sealed class GetCoursesQuery : IRequest<GetCoursesQueryResult>
{
    public string? Keyword { get; set; }

    public List<int> RouteIds { get; set; } = [];

    public List<int> Levels { get; set; } = [];
    public List<LearningType> LearningTypes { get; set; } = [];
    public int? Distance { get; set; }

    public string? LocationName { get; set; }

    public CoursesOrderBy OrderBy { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}
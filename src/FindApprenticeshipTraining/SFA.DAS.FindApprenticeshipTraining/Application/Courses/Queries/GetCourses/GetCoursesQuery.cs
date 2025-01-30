﻿using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.GetCourses;

#nullable enable

public sealed class GetCoursesQuery : IRequest<GetCoursesQueryResult>
{
    public string? Keyword { get; set; }

    public List<int> RouteIds { get; set; } = new List<int>();

    public List<int> Levels { get; set; } = new List<int>();

    public CoursesOrderBy? OrderBy { get; set; }

    public int? Distance { get; set; }

    public decimal? Longitude { get; set; }

    public decimal? Latitude { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}


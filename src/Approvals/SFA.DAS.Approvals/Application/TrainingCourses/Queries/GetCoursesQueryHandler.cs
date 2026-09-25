using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Linq;

namespace SFA.DAS.Approvals.Application.TrainingCourses.Queries;

public class GetCoursesQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
    : IRequestHandler<GetCoursesQuery, GetCoursesResult>
{
    public async Task<GetCoursesResult> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
    {
        var (activeCourses, allOldCourses) = await GetCoursesAsync();

        var activeLarsCodes = activeCourses.Select(c => c.LarsCode).ToHashSet();

        var earliestEffectiveFromByLarsCode = allOldCourses
            .Where(c => c.CourseDates?.EffectiveFrom != null)
            .GroupBy(c => c.LarsCode)
            .ToDictionary(
            g => g.Key,
            g => g.Min(c => c.CourseDates.EffectiveFrom));

        var latestOldCourses = allOldCourses
            .Where(c => !activeLarsCodes.Contains(c.LarsCode))
            .GroupBy(c => c.LarsCode)
            .Select(g => g.MaxBy(c => c.CourseDates?.EffectiveTo ?? DateTime.MaxValue)!)
            .ToList();

        var allCourses = activeCourses.Concat(latestOldCourses).ToList();
        foreach (var course in allCourses)
        {
            if (course.CourseDates != null && earliestEffectiveFromByLarsCode.TryGetValue(course.LarsCode, out var earliestEffectiveFrom))
            {
                course.CourseDates.EffectiveFrom = earliestEffectiveFrom;
            }
        }
        return new GetCoursesResult
        {
            Courses = allCourses
        };
    }

    private async Task<(List<GetCoursesListItem> ActiveCourses, List<GetCoursesListItem> OldCourses)> GetCoursesAsync()
    {
        var activeCoursesTask = coursesApiClient.Get<GetCoursesListResponse>(new GetCoursesExportRequest());

        var oldCoursesTask = coursesApiClient.Get<GetCoursesListResponse>(new GetOldCoursesRequest());

        await Task.WhenAll(activeCoursesTask, oldCoursesTask);

        return
        (
            ActiveCourses: (await activeCoursesTask).Courses.ToList(),
            OldCourses: (await oldCoursesTask).Courses.ToList()
        );
    }
}
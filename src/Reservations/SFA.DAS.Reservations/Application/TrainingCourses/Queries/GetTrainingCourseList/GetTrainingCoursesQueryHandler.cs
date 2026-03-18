using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Reservations.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Reservations.Application.TrainingCourses.Queries.GetTrainingCourseList
{
    public class GetTrainingCoursesQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        : IRequestHandler<GetTrainingCoursesQuery, GetTrainingCoursesResult>
    {
        public async Task<GetTrainingCoursesResult> Handle(GetTrainingCoursesQuery request, CancellationToken cancellationToken)
        {
            var standardsTask = coursesApiClient.Get<GetStandardsListResponse>(new GetAvailableToStartStandardsListRequest());
            var shortCoursesTask = coursesApiClient.Get<GetCoursesSearchApiResponse>(new GetCoursesSearchRequest());

            await Task.WhenAll(standardsTask, shortCoursesTask);

            var standards = (await standardsTask).Standards ?? [];
            var shortCoursesSearchResponse = await shortCoursesTask;
            var shortCourseListItems = MapCoursesSearchToShortCourseListItems(shortCoursesSearchResponse?.Courses ?? []);
            var standardCourses = MapStandardsToTrainingCourseListItem(standards);
            var shortCourses = MapShortCoursesToTrainingCourseListItem(shortCourseListItems);

            var combined = standardCourses.Concat(shortCourses).ToList();

            return new GetTrainingCoursesResult
            {
                Courses = combined
            };
        }

        private static IEnumerable<ShortCourseListItem> MapCoursesSearchToShortCourseListItems(IEnumerable<CourseSearchItemDto> courses)
        {
            return courses.Select(c => new ShortCourseListItem
            {
                CourseUId = c.StandardUId,
                ReferenceNumber = c.LarsCode,
                LarsCode = c.LarsCode,
                Title = c.Title ?? string.Empty,
                LevelCode = c.Level.ToString(),
                CourseDates = c.CourseDates != null
                    ? new ShortCourseDates
                    {
                        EffectiveFrom = c.CourseDates.EffectiveFrom,
                        EffectiveTo = c.CourseDates.EffectiveTo
                    }
                    : null,
                CourseType = "ShortCourse",
                LearningType = "ApprenticeshipUnit"
            });
        }

        private static IEnumerable<TrainingCourseListItem> MapStandardsToTrainingCourseListItem(IEnumerable<GetStandardsListItem> standards)
        {
            return standards.Select(s => new TrainingCourseListItem
            {
                StandardUId = s.StandardUId,
                LarsCode = s.LarsCode.ToString(),
                Title = s.Title ?? string.Empty,
                Level = s.Level.ToString(),
                EffectiveTo = s.EffectiveTo,
                LearningType = s.ApprenticeshipType
            });
        }

        private static IEnumerable<TrainingCourseListItem> MapShortCoursesToTrainingCourseListItem(IEnumerable<ShortCourseListItem> shortCourses)
        {
            return shortCourses.Select(c => new TrainingCourseListItem
            {
                StandardUId = c.CourseUId,
                LarsCode = c.LarsCode,
                Title = c.Title ?? string.Empty,
                Level = c.LevelCode,
                EffectiveTo = c.CourseDates?.EffectiveTo ?? default,
                LearningType = c.LearningType
            });
        }
    }
}
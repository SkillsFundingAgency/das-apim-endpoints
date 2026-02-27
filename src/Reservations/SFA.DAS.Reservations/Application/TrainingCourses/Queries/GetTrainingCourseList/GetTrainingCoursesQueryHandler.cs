using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Reservations.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Reservations.Application.TrainingCourses.Queries.GetTrainingCourseList
{
    public class GetTrainingCoursesQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        : IRequestHandler<GetTrainingCoursesQuery, GetTrainingCoursesResult>
    {
        public async Task<GetTrainingCoursesResult> Handle(GetTrainingCoursesQuery request, CancellationToken cancellationToken)
        {
            var standardsTask = coursesApiClient.Get<GetStandardsListResponse>(new GetAvailableToStartStandardsListRequest());
            var shortCoursesTask = Task.Run(() => new GetShortCoursesListResponse
            {
                Courses = new List<ShortCourseListItem>
                {
                    new()
                    {
                        CourseUId = "SC0002_1.0",
                        ReferenceNumber = "SC0002",
                        LarsCode = "ZSC00002",
                        Title = "Teacher Assistant - Apprenticeship Unit",
                        LevelCode = "4",
                        CourseDates = new ShortCourseDates
                        {
                            EffectiveTo = null,
                            EffectiveFrom = new DateTime(2026, 1, 1)
                        },
                        CourseType = "ShortCourse",
                        LearningType = "ApprenticeshipUnit"
                    }
                }
            }, cancellationToken); // coursesApiClient.Get<GetShortCoursesListResponse>(new GetShortCoursesListRequest());
            
            await Task.WhenAll(standardsTask, shortCoursesTask);
            
            var standards = (await standardsTask).Standards ?? [];
            var shortCoursesResponse = await shortCoursesTask;
            var standardCourses = MapStandardsToTrainingCourseListItem(standards);
            var shortCourses = MapShortCoursesToTrainingCourseListItem(shortCoursesResponse.Courses ?? []);

            var combined = standardCourses.Concat(shortCourses).ToList();

            return new GetTrainingCoursesResult
            {
                Courses = combined
            };
        }

        private static IEnumerable<TrainingCourseListItem> MapStandardsToTrainingCourseListItem(IEnumerable<GetStandardsListItem> standards)
        {
            foreach (var s in standards)
            {
                yield return new TrainingCourseListItem
                {
                    StandardUId = s.StandardUId,
                    LarsCode = s.LarsCode.ToString(),
                    Title = s.Title ?? string.Empty,
                    Level = s.Level.ToString(),
                    EffectiveTo = s.EffectiveTo,
                    LearningType = s.ApprenticeshipType
                };
            }
        }

        private static IEnumerable<TrainingCourseListItem> MapShortCoursesToTrainingCourseListItem(IEnumerable<ShortCourseListItem> shortCourses)
        {
            foreach (var c in shortCourses)
            {
                yield return new TrainingCourseListItem
                {
                    StandardUId = c.CourseUId,
                    LarsCode = c.LarsCode,
                    Title = c.Title ?? string.Empty,
                    Level = c.LevelCode,
                    EffectiveTo = c.CourseDates?.EffectiveTo ?? default,
                    LearningType = c.LearningType
                };
            }
        }
    }
}
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.VacanciesManage.Api.Models;
using SFA.DAS.VacanciesManage.Application.TrainingCourses.Queries;

namespace SFA.DAS.VacanciesManage.Api.UnitTests.Models
{
    public class WhenMappingFromMediatorResponseToGetTrainingCoursesListResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetTrainingCoursesQueryResult source)
        {
            var actual = (GetTrainingCoursesListResponse) source;

            actual.TrainingCourses.Select(c => c.LarsCode).ToList().Should()
                .BeEquivalentTo(source.TrainingCourses.Select(c => c.LarsCode).ToList());
            actual.TrainingCourses.Select(c => c.Route).ToList().Should()
                .BeEquivalentTo(source.TrainingCourses.Select(c => c.Route).ToList());
            actual.TrainingCourses.Select(c => c.Title).ToList().Should()
                .BeEquivalentTo(source.TrainingCourses.Select(c => $"{c.Title} (level {c.Level})").ToList());
        }
    }
}
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Vacancies.Api.Models;
using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Api.UnitTests.Models
{
    public class WhenMappingFromMediatorResponseToGetVacancyCourseItem
    {
        [Test, AutoData]
        public void Then_The_Values_Are_Mapped(GetVacanciesListItem source)
        {
            var actual = (GetVacancyCourseItem)source;
            
            actual.Title.Should().Be($"{source.CourseTitle} (level {source.CourseLevel})");
            actual.Level.Should().Be(source.CourseLevel);
            actual.Route.Should().Be(source.Route);
            actual.LarsCode.Should().Be(source.StandardLarsCode);
        }

        [Test, AutoData]
        public void Then_If_Course_Is_Null_Then_Not_Mapped(GetVacanciesListItem source)
        {
            source.StandardLarsCode = null;
            
            var actual = (GetVacancyCourseItem)source;

            actual.Title.Should().BeNullOrEmpty();
            actual.Route.Should().BeNullOrEmpty();
            actual.Level.Should().Be(0);
            actual.LarsCode.Should().Be(0);
        }
    }
}
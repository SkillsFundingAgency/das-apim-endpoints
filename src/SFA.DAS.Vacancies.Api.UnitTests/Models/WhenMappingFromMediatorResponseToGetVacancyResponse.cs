using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Vacancies.Api.Models;
using SFA.DAS.Vacancies.Application.Vacancies.Queries;

namespace SFA.DAS.Vacancies.Api.UnitTests.Models
{
    public class WhenMappingFromMediatorResponseToGetVacancyResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetVacancyQueryResult source)
        {
            source.Vacancy.WageType = 3;
            source.Vacancy.WageUnit = 1;
            
            var actual = (GetVacancyResponse)source;
            
            actual.Should().BeEquivalentTo(source.Vacancy, options => options
                .Excluding(c=>c.CourseLevel)
                .Excluding(c=>c.CourseTitle)
                .Excluding(c=>c.Route)
                .Excluding(c=>c.StandardLarsCode)
                .Excluding(c=>c.LongDescription)
                .Excluding(c=>c.Qualifications)
                .Excluding(c=>c.FrameworkLarsCode)
                .Excluding(c=>c.WageAmountLowerBound)
                .Excluding(c=>c.WageAmountUpperBound)
                .Excluding(c=>c.WageText)
                .Excluding(c=>c.WageType)
                .Excluding(c=>c.WageAmount)
                .Excluding(c=>c.WageUnit)
                .Excluding(c=>c.Id)
                .Excluding(c=>c.AnonymousEmployerName)
                .Excluding(c=>c.Category)
                .Excluding(c=>c.CategoryCode)
                .Excluding(c=>c.IsEmployerAnonymous)
                .Excluding(c=>c.SubCategory)
                .Excluding(c=>c.SubCategoryCode)
                .Excluding(c=>c.VacancyLocationType)
                .Excluding(c=>c.WorkingWeek)
                .Excluding(c=>c.Score)
                .Excluding(c=>c.IsPositiveAboutDisability)
            );
            actual.FullDescription.Should().Be(source.Vacancy.LongDescription);
            actual.Qualifications.Should().BeEquivalentTo(source.Vacancy.Qualifications.Select(c=>(GetVacancyQualification)c).ToList());
            actual.Location.Lat.Should().Be(source.Vacancy.Location.Lat);
            actual.Location.Lon.Should().Be(source.Vacancy.Location.Lon);
            actual.Course.Level.Should().Be(source.Vacancy.CourseLevel);
            actual.Course.Title.Should().Be($"{source.Vacancy.CourseTitle} (level {source.Vacancy.CourseLevel})");
            actual.Course.Route.Should().Be(source.Vacancy.Route);
            actual.Course.LarsCode.Should().Be(source.Vacancy.StandardLarsCode);
            actual.Wage.WageAmount.Should().Be(source.Vacancy.WageAmount);
            actual.Wage.WageType.Should().Be(source.Vacancy.WageType);
            actual.Wage.WageUnit.Should().Be(source.Vacancy.WageUnit);
            actual.Wage.WageAdditionalInformation.Should().Be(source.Vacancy.WageText);
            actual.Wage.WageAmountLowerBound.Should().Be(source.Vacancy.WageAmountLowerBound);
            actual.Wage.WageAmountUpperBound.Should().Be(source.Vacancy.WageAmountUpperBound);
        }

        [Test, AutoData]
        public void Then_If_Anonymous_Then_Anon_Values_Mapped(GetVacancyQueryResult source)
        {
            source.Vacancy.IsEmployerAnonymous = true;
            source.Vacancy.VacancyLocationType = "nATIonal";
            
            var actual = (GetVacancyResponse)source;
            
            actual.Should().BeEquivalentTo(source.Vacancy,options => options
                .ExcludingMissingMembers()
                .Excluding(item => item.EmployerName)
                .Excluding(item => item.CourseTitle)
                .Excluding(item => item.CourseLevel)
                .Excluding(item => item.Location)
            );
            actual.EmployerName.Should().Be(source.Vacancy.AnonymousEmployerName);
            actual.Location.Should().BeNull();
        }

        [Test, AutoData]
        public void Then_If_Null_Then_Null_Returned(GetVacancyQueryResult source)
        {
            source.Vacancy = null;
            
            var actual = (GetVacancyResponse)source;

            actual.Should().BeNull();
        }
    }
}
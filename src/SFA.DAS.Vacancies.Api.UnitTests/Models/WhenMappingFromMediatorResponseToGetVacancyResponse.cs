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
            
            var actual = (GetVacancyResponse)source;
            
            actual.Should().BeEquivalentTo(source.Vacancy,options => options
                .ExcludingMissingMembers());
            actual.FullDescription.Should().Be(source.Vacancy.LongDescription);
            actual.Qualifications.Should().BeEquivalentTo(source.Vacancy.Qualifications.Select(c=>(GetVacancyQualification)c).ToList());
            actual.Wage.WageAmount.Should().Be(source.Vacancy.WageAmount);
            actual.Wage.WageType.Should().Be((WageType)source.Vacancy.WageType);
            actual.Wage.WageAdditionalInformation.Should().Be(source.Vacancy.WageText);
            actual.Wage.WageAmountUpperBound.Should().Be(source.Vacancy.WageAmountUpperBound);
            actual.Wage.WageAmountLowerBound.Should().Be(source.Vacancy.WageAmountLowerBound);
            actual.Wage.WorkingWeekDescription.Should().Be(source.Vacancy.WorkingWeek);
            
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
                .Excluding(item => item.CourseLevel));
            actual.EmployerName.Should().Be(source.Vacancy.AnonymousEmployerName);
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
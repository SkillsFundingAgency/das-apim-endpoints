using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Vacancies.Api.Models;
using SFA.DAS.Vacancies.Application.Vacancies.Queries;

namespace SFA.DAS.Vacancies.Api.UnitTests.Models
{
    public class WhenMappingFromMediatorResponseToGetTraineeshipVacancyResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetTraineeshipVacancyQueryResult source)
        {
            var actual = (GetTraineeshipVacancyResponse)source;
            
            actual.Should().BeEquivalentTo(source.Vacancy, options => options
                .Excluding(c=>c.RouteId)
                .Excluding(c=>c.LongDescription)
                .Excluding(c=>c.Id)
                .Excluding(c=>c.AnonymousEmployerName)
                .Excluding(c=>c.Category)
                .Excluding(c=>c.CategoryCode)
                .Excluding(c=>c.IsEmployerAnonymous)
                .Excluding(c=>c.VacancyLocationType)
                .Excluding(c=>c.WorkingWeek)
                .Excluding(c=>c.Score)
                .Excluding(c=>c.IsPositiveAboutDisability)
            );
            actual.FullDescription.Should().Be(source.Vacancy.LongDescription);
            actual.Location.Lat.Should().Be(source.Vacancy.Location.Lat);
            actual.Location.Lon.Should().Be(source.Vacancy.Location.Lon);
        }

        [Test, AutoData]
        public void Then_If_Anonymous_Then_Anon_Values_Mapped(GetTraineeshipVacancyQueryResult source)
        {
            source.Vacancy.IsEmployerAnonymous = true;
            source.Vacancy.VacancyLocationType = "nATIonal";
            
            var actual = (GetTraineeshipVacancyResponse)source;
            
            actual.Should().BeEquivalentTo(source.Vacancy,options => options
                .ExcludingMissingMembers()
                .Excluding(item => item.EmployerName)
                .Excluding(item => item.Location)
            );
            actual.EmployerName.Should().Be(source.Vacancy.AnonymousEmployerName);
            actual.Location.Should().BeNull();
        }

        [Test, AutoData]
        public void Then_If_Null_Then_Null_Returned(GetTraineeshipVacancyQueryResult source)
        {
            source.Vacancy = null;
            
            var actual = (GetTraineeshipVacancyResponse)source;

            actual.Should().BeNull();
        }
    }
}
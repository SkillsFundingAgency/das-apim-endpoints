using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Vacancies.Api.Models;
using SFA.DAS.Vacancies.Application.Vacancies.Queries;

namespace SFA.DAS.Vacancies.Api.UnitTests.Models
{
    public class WhenMappingFromMediatorResponseToGetTraineeshipVacanciesListResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetTraineeshipVacanciesQueryResult source)
        {
            var actual = (GetTraineeshipVacanciesListResponse)source;

            actual.Vacancies.Should().BeEquivalentTo(source.Vacancies, options => options.ExcludingMissingMembers().Excluding(c => c.EmployerName));
            actual.Total.Should().Be(source.Total);
            actual.TotalFiltered.Should().Be(source.TotalFiltered);
            actual.TotalPages.Should().Be(source.TotalPages);
            foreach (var vacancy in actual.Vacancies)
            {
                var expectedVacancy =
                    source.Vacancies.Single(c => c.VacancyReference.Equals(vacancy.VacancyReference));
                vacancy.Location.Lat.Should().Be(expectedVacancy.Location.Lat);
                vacancy.Location.Lon.Should().Be(expectedVacancy.Location.Lon);
            }

        }

        [Test, AutoData]
        public void And_IsEmployerAnonymous_Then_Anon_Values_Used(GetTraineeshipVacanciesQueryResult source)
        {
            //arrange
            var sourceVacancies = source.Vacancies.ToList();
            foreach (var getVacanciesItem in sourceVacancies)
            {
                getVacanciesItem.IsEmployerAnonymous = true;
                getVacanciesItem.VacancyLocationType = "nAtiONal";
            }

            //act
            var actual = (GetTraineeshipVacanciesListResponse)source;

            //assert
            actual.Vacancies.Should().BeEquivalentTo(sourceVacancies, options => options
                .ExcludingMissingMembers()
                .Excluding(item => item.EmployerName)
                .Excluding(item => item.Location));
            actual.Vacancies.TrueForAll(c => c.IsNationalVacancy).Should().BeTrue();
            for (var i = 0; i < actual.Vacancies.Count; i++)
            {
                actual.Vacancies[i].EmployerName.Should().Be(sourceVacancies[i].AnonymousEmployerName);
                actual.Vacancies[i].Location.Lat.Should().Be(sourceVacancies[i].Location.Lat);
                actual.Vacancies[i].Location.Lon.Should().Be(sourceVacancies[i].Location.Lon);
            }
        }
    }
}

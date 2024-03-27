using System.Linq;
using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.Vacancies.Api.Models;
using SFA.DAS.Vacancies.Application.Vacancies.Queries;
using FluentAssertions;

namespace SFA.DAS.Vacancies.Api.UnitTests.Models
{
    public class WhenMappingFromMediatorResponseToGetVacanciesListResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetVacanciesQueryResult source, int ukprn)
        {
            foreach (var vacancy in source.Vacancies)
            {
                vacancy.Ukprn = ukprn.ToString();
            }

            var actual = (GetVacanciesListResponse) source;

            actual.Vacancies.Should().BeEquivalentTo(source.Vacancies, options => options.ExcludingMissingMembers()
                        .Excluding(c => c.EmployerName)
                        .Excluding(c => c.Ukprn));
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
        public void And_IsEmployerAnonymous_Then_Anon_Values_Used(GetVacanciesQueryResult source, int ukprn)
        {
            //arrange
            var sourceVacancies = source.Vacancies.ToList();
            foreach (var getVacanciesItem in sourceVacancies)
            {
                getVacanciesItem.Ukprn = ukprn.ToString();
                getVacanciesItem.IsEmployerAnonymous = true;
                getVacanciesItem.VacancyLocationType = "nAtiONal";
            }
            
            //act
            var actual = (GetVacanciesListResponse) source ;

            //assert
            actual.Vacancies.Should().BeEquivalentTo(sourceVacancies, options => options
                .ExcludingMissingMembers()
                .Excluding(item => item.Ukprn)
                .Excluding(item => item.EmployerName)
                .Excluding(item => item.CourseTitle)
                .Excluding(item => item.CourseLevel)
                .Excluding(item => item.Location));
            actual.Vacancies.TrueForAll(c => c.IsNationalVacancy).Should().BeTrue();
            for (var i = 0; i < actual.Vacancies.Count; i++)
            {
                actual.Vacancies[i].Ukprn.Should().Be(int.Parse(sourceVacancies[i].Ukprn));
                actual.Vacancies[i].EmployerName.Should().Be(sourceVacancies[i].AnonymousEmployerName);
                actual.Vacancies[i].Course.Title.Should().Be($"{sourceVacancies[i].CourseTitle} (level {sourceVacancies[i].CourseLevel})");
                actual.Vacancies[i].Course.Level.Should().Be(sourceVacancies[i].CourseLevel);
                actual.Vacancies[i].Course.Route.Should().Be(sourceVacancies[i].Route);
                actual.Vacancies[i].Course.LarsCode.Should().Be(sourceVacancies[i].StandardLarsCode);
                actual.Vacancies[i].Location.Lat.Should().Be(sourceVacancies[i].Location.Lat);
                actual.Vacancies[i].Location.Lon.Should().Be(sourceVacancies[i].Location.Lon); 
            }
        }
    }
}

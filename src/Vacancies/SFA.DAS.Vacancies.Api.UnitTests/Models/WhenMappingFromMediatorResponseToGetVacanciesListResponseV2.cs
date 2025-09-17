using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Vacancies.Api.Models;
using SFA.DAS.Vacancies.Application.Vacancies.Queries.GetVacancies;
using System;
using System.Linq;

namespace SFA.DAS.Vacancies.Api.UnitTests.Models
{
    public class WhenMappingFromMediatorResponseToGetVacanciesListResponseV2
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetVacanciesQueryResult source, int ukprn)
        {
            foreach (var vacancy in source.Vacancies)
            {
                vacancy.Ukprn = ukprn.ToString();
            }

            var actual = (GetVacanciesListResponseV2) source;

            actual.Vacancies.Should().BeEquivalentTo(source.Vacancies, options => options.ExcludingMissingMembers()
                        .Excluding(c => c.EmployerName)
                        .Excluding(c => c.Ukprn)
                        .Excluding(item => item.ClosingDate)
                        .Excluding(c => c.VacancyReference)
                        .Excluding(item => item.Location)
                        .Excluding(item => item.OtherAddresses)
            );
            actual.Total.Should().Be(source.Total);
            actual.TotalFiltered.Should().Be(source.TotalFiltered);
            actual.TotalPages.Should().Be(source.TotalPages);
            foreach (var vacancy in actual.Vacancies)
            {
                var expectedVacancy =
                    source.Vacancies.Single(c => c.VacancyReference.Equals(vacancy.VacancyReference.Replace("VAC","")));
                vacancy.ClosingDate.Should().Be(expectedVacancy.ClosingDate.AddDays(1).Subtract(TimeSpan.FromSeconds(1)));
                vacancy.ApplicationUrl.Should().Be(expectedVacancy.ApplicationUrl);
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
            var actual = (GetVacanciesListResponseV2) source ;

            //assert
            actual.Vacancies.Should().BeEquivalentTo(sourceVacancies, options => options
                .ExcludingMissingMembers()
                .Excluding(item => item.Ukprn)
                .Excluding(item => item.EmployerName)
                .Excluding(item => item.CourseTitle)
                .Excluding(item => item.CourseLevel)
                .Excluding(item => item.ClosingDate)
                .Excluding(item => item.Location)
                .Excluding(item => item.OtherAddresses));
            actual.Vacancies.TrueForAll(c => c.IsNationalVacancy).Should().BeTrue();
            for (var i = 0; i < actual.Vacancies.Count; i++)
            {
                actual.Vacancies[i].Ukprn.Should().Be(int.Parse(sourceVacancies[i].Ukprn));
                actual.Vacancies[i].EmployerName.Should().Be(sourceVacancies[i].AnonymousEmployerName);
                actual.Vacancies[i].Course.Title.Should().Be($"{sourceVacancies[i].CourseTitle} (level {sourceVacancies[i].CourseLevel})");
                actual.Vacancies[i].Course.Level.Should().Be(sourceVacancies[i].CourseLevel);
                actual.Vacancies[i].Course.Route.Should().Be(sourceVacancies[i].Route);
                actual.Vacancies[i].Course.LarsCode.Should().Be(sourceVacancies[i].StandardLarsCode);
                actual.Vacancies[i].ApplicationUrl.Should().Be(sourceVacancies[i].ApplicationUrl);
            }
        }
    }
}

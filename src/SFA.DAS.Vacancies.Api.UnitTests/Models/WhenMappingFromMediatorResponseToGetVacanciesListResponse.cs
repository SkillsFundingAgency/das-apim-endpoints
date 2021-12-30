using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.Vacancies.Api.Models;
using SFA.DAS.Vacancies.Application.Vacancies.Queries;
using FluentAssertions;
using SFA.DAS.Vacancies.InnerApi.Responses;
using StructureMap.Diagnostics;

namespace SFA.DAS.Vacancies.Api.UnitTests.Models
{
    class WhenMappingFromMediatorResponseToGetVacanciesListResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_are_mapped(GetVacanciesQueryResult source)
        {
            var actual = (GetVacanciesListResponse) source ;

            actual.Should().BeEquivalentTo(source, options => options.ExcludingMissingMembers());
        }
        
        [Test, AutoData]
        public void And_IsEmployerAnonymous_Then_Anon_Values_Used(GetVacanciesQueryResult source)
        {
            //arrange
            var sourceVacancies = source.Vacancies.ToList();
            foreach (var getVacanciesItem in sourceVacancies)
            {
                getVacanciesItem.IsEmployerAnonymous = true;
            }
            
            //act
            var actual = (GetVacanciesListResponse) source ;

            //assert
            actual.Vacancies.Should().BeEquivalentTo(sourceVacancies, options => options
                .ExcludingMissingMembers()
                .Excluding(item => item.EmployerName));
            for (var i = 0; i < actual.Vacancies.Count; i++)
            {
                actual.Vacancies[i].EmployerName.Should().Be(sourceVacancies[i].AnonymousEmployerName);
            }
        }
    }
}

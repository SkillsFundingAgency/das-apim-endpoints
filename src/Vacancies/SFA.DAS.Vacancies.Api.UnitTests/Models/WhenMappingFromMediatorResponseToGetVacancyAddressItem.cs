using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Vacancies.Api.Models;
using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Api.UnitTests.Models
{
    public class WhenMappingFromMediatorResponseToGetVacancyAddressItem
    {
        [Test, AutoData]
        public void Then_The_Values_Are_Mapped(GetVacanciesListItem source)
        {
            // act
            var actual = GetVacancyAddressItem.From(source.Address);
            
            // assert
            actual.Should().BeEquivalentTo(source.Address);
        }
        
        [Test, AutoData]
        public void Then_Empty_Object_Returned_If_Null(GetVacanciesListItem source)
        {
            // act
            var actual = GetVacancyAddressItem.From(null);
            
            // assert
            actual.Should().BeEquivalentTo(new GetVacancyAddressItem());
        }
    }
}
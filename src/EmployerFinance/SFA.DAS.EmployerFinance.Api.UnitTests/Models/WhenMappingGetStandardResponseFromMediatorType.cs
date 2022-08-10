using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Api.Models;
using SFA.DAS.EmployerFinance.InnerApi.Responses;

namespace SFA.DAS.EmployerFinance.Api.UnitTests.Models
{
    public class WhenMappingGetStandardResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetStandardsListItem source)
        {
            //Arrange
            var actual = (StandardResponse)source;

            //Assert
            actual.Should().BeEquivalentTo(source, options=> options
                .Excluding(x=>x.ApprenticeshipFunding)
                .Excluding(x=>x.StandardDates)
                .Excluding(x=>x.TypicalDuration)
                .Excluding(x=>x.IsActive)
                .Excluding(x => x.StandardUId)
                .Excluding(x => x.LarsCode)
            );
            actual.Duration.Should().Be(source.TypicalDuration);
            actual.Id.Should().Be(source.LarsCode);
        }
    }
}
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ManageApprenticeships.Api.Models;
using SFA.DAS.ManageApprenticeships.InnerApi.Responses;

namespace SFA.DAS.ManageApprenticeships.Api.UnitTests.Models
{
    public class WhenMappingGetFrameworkResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetFrameworksListItem source)
        {
            //Arrange
            var actual = (GetFrameworkResponse) source;
            
            //Assert
            actual.Should().BeEquivalentTo(source, options=> options
                .Excluding(x=>x.FundingPeriods)
                .Excluding(x=>x.IsActiveFramework)
                .Excluding(x=>x.CurrentFundingCap)
            );
            actual.MaxFunding.Should().Be(source.CurrentFundingCap);
        }
    }
}
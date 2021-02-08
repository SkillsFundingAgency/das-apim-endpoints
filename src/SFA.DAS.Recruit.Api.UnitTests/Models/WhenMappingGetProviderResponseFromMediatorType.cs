using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.Api.UnitTests.Models
{
    public class WhenMappingGetProviderResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetProvidersListItem source)
        {
            //Arrange
            var actual = (GetProviderResponse) source;
            
            //Assert
            actual.Should().BeEquivalentTo(source);
        }
    }
}
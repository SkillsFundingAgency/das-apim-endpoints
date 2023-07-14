using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Recruit.InnerApi.Responses;
using GetProviderResponse = SFA.DAS.Recruit.Api.Models.GetProviderResponse;

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
            actual.Should().BeEquivalentTo(source, options => options
                .Excluding(x => x.Address.AddressLine1)
                .Excluding(x => x.Address.AddressLine2)
                .Excluding(x => x.Address.AddressLine3)
                .Excluding(x => x.Address.AddressLine4)
            );

            actual.Address.Address1.Should().Be(source.Address.AddressLine1);
            actual.Address.Address2.Should().Be(source.Address.AddressLine2);
            actual.Address.Address3.Should().Be(source.Address.AddressLine3);
            actual.Address.Address4.Should().Be(source.Address.AddressLine4);
        }
    }
}
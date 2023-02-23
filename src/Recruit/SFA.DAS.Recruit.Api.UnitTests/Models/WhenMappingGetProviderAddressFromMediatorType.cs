using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.Api.UnitTests.Models
{
    public class WhenMappingGetProviderAddressFromMediatorType
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetProvidersListItemAddress source)
        {
            //Arrange
            var actual = (GetProviderAddress) source;
            
            //Assert
            actual.Should().BeEquivalentTo(source, options => options
                .Excluding(x => x.AddressLine1)
                .Excluding(x => x.AddressLine2)
                .Excluding(x => x.AddressLine3)
                .Excluding(x => x.AddressLine4)
            );

           actual.Address1.Should().Be(source.AddressLine1);
           actual.Address2.Should().Be(source.AddressLine2); 
           actual.Address3.Should().Be(source.AddressLine3); 
           actual.Address4.Should().Be(source.AddressLine4);
        }

        [Test]
        public void Then_Null_Address_Is_Mapped_To_Empty_Model()
        {
            var source = (GetProvidersListItemAddress)null;
            //Arrange
            var actual = (GetProviderAddress)source;

            //Assert
            actual.Should().BeEquivalentTo(new GetProviderAddress());
        }
    }
}
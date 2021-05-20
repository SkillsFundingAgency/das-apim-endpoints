using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Api.Models;
using SFA.DAS.EmployerDemand.InnerApi.Responses;

namespace SFA.DAS.EmployerDemand.Api.UnitTests.Models
{
    public class WhenCastingFromGetProviderCourseInformationToGetProviderContactDetails
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetProviderCourseInformation source)
        {
            //Arrange
            source.TradingName = null;

            //Act
            var actual = (GetProviderContactDetails)source;
            
            //Assert
            actual.Ukprn.Should().Be(source.Ukprn);
            actual.Website.Should().Be(source.ContactUrl);
            actual.PhoneNumber.Should().Be(source.Phone);
            actual.EmailAddress.Should().Be(source.Email);
            actual.Name.Should().Be(source.Name);
        }

        [Test, AutoData]
        public void And_Has_TradingName_Then_Used_For_Name(GetProviderCourseInformation source)
        {
            //Act
            var actual = (GetProviderContactDetails)source;
            
            //Assert
            actual.Name.Should().Be(source.TradingName);
        }

        [Test]
        public void Then_If_Null_Then_Null_Returned()
        {
            //Act
            var actual = (GetProviderContactDetails)(GetProviderCourseInformation)null;
            
            //Assert
            actual.Should().BeNull();
        }
    }
}
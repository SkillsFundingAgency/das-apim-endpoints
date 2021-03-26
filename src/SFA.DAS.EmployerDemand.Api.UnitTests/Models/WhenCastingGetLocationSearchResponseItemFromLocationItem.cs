using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Api.Models;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmployerDemand.Api.UnitTests.Models
{
    public class WhenCastingGetLocationSearchResponseItemFromLocationItem
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(LocationItem source)
        {
            //Act
            var actual = (GetLocationSearchResponseItem) source;
            
            //Act
            actual.Name.Should().Be(source.Name);
            actual.Location.GeoPoint.Should().BeEquivalentTo(source.GeoPoint);
        }

        [Test]
        public void Then_Null_Returns_Null()
        {
            //Act
            var actual = (GetLocationSearchResponseItem) (LocationItem)null;
            
            //Act
            actual.Should().BeNull();
        }
    }
}
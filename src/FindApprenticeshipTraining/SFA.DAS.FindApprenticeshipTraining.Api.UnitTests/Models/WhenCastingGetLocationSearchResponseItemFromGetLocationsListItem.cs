using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Models
{
    public class WhenCastingGetLocationSearchResponseItemFromGetLocationsListItem
    {
        [Test, AutoData]
        public void Then_The_Fields_are_Correctly_Mapped_When_Postcode_Is_Null(GetLocationsListItem source)
        {
            //Arrange
            source.Postcode = null;
            source.DistrictName = null;

            //Act
            var actual = (GetLocationSearchResponseItem)source;

            //Assert
            actual.Name.Should().Be($"{source.LocationName}, {source.LocalAuthorityName}");
            actual.Location.Should().BeEquivalentTo(source.Location);
        }

        [Test, AutoData]
        public void Then_The_Fields_are_Correctly_Mapped_When_Postcode_Is_Empty(GetLocationsListItem source)
        {
            //Arrange
            source.Postcode = "";
            source.DistrictName = null;

            //Act
            var actual = (GetLocationSearchResponseItem)source;

            //Assert
            actual.Name.Should().Be($"{source.LocationName}, {source.LocalAuthorityName}");
            actual.Location.Should().BeEquivalentTo(source.Location);
        }

        [Test, AutoData]
        public void Then_The_Fields_are_Correctly_Mapped_When_Postcode_Is_Not_Null(GetLocationsListItem source)
        {
            //Act
            source.DistrictName = null;
            source.IncludeDistrictNameInPostcodeDisplayName = false;
            var actual = (GetLocationSearchResponseItem)source;

            //Assert
            actual.Name.Should().Be(source.Postcode);
            actual.Location.Should().BeEquivalentTo(source.Location);
        }
    }
}

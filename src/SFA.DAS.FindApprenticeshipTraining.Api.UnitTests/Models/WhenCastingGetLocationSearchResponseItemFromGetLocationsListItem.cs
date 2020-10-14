using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.Application.Locations.GetLocations;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using static SFA.DAS.FindApprenticeshipTraining.Api.Models.GetLocationSearchResponse;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Models
{
    public class WhenCastingGetLocationSearchResponseItemFromGetLocationsListItem
    {
        [Test, AutoData]
        public void Then_The_Fields_are_Correctly_Mapped_When_Postcode_Is_Null(GetLocationsListItem source)
        {
            //Arrange
            source.Postcode = null;

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
            var actual = (GetLocationSearchResponseItem)source;

            //Assert
            actual.DistrictName.Should().Be(source.DistrictName);
            actual.Name.Should().Be(source.Postcode);
            actual.Location.Should().BeEquivalentTo(source.Location);
        }
    }
}

using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Location;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Models
{
    public class GetLocationsListItemTests
    {
        [Test, AutoData]
        public void WhenMappingGetLocationsListItem_AndPostcodeIsNull_ThenFieldsAreCorrectlyMapped(GetLocationsListItem source)
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
        public void WhenMappingGetLocationsListItem_AndPostcodeIsEmpty_ThenFieldsAreCorrectlyMapped(GetLocationsListItem source)
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
        public void WhenMappingGetLocationsListItem_AndPostcodeIsNotNull_ThenFieldsAreCorrectlyMapped(GetLocationsListItem source)
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

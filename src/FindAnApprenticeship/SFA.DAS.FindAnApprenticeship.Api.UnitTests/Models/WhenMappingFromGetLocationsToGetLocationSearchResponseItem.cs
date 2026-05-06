using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Location;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models
{
    public class WhenMappingFromGetLocationsToGetLocationSearchResponseItem
    {
        [Test, AutoData]
        public void Then_The_Fields_are_Correctly_Mapped_When_Postcode_Is_Null(GetLocationsListItem source)
        {
            source.Postcode = null;
            source.DistrictName = null;

            var actual = (GetLocationSearchResponseItem)source;

            using (new AssertionScope())
            {
                actual.Name.Should().Be($"{source.LocationName}, {source.LocalAuthorityName}");
                actual.Location.Should().BeEquivalentTo(source.Location);
            }
        }

        [Test, AutoData]
        public void Then_The_Fields_are_Correctly_Mapped_When_Postcode_Is_Empty(GetLocationsListItem source)
        {
            source.Postcode = "";
            source.DistrictName = null;

            var actual = (GetLocationSearchResponseItem)source;

            using (new AssertionScope())
            {
                actual.Name.Should().Be($"{source.LocationName}, {source.LocalAuthorityName}");
                actual.Location.Should().BeEquivalentTo(source.Location);
            }
        }

        [Test, AutoData]
        public void Then_The_Fields_are_Correctly_Mapped_When_Postcode_Is_Not_Null(GetLocationsListItem source)
        {
            source.DistrictName = null;
            source.IncludeDistrictNameInPostcodeDisplayName = false;

            var actual = (GetLocationSearchResponseItem)source;

            using (new AssertionScope())
            {
                actual.Name.Should().Be(source.Postcode);
                actual.Location.Should().BeEquivalentTo(source.Location);
            }
        }
    }
}

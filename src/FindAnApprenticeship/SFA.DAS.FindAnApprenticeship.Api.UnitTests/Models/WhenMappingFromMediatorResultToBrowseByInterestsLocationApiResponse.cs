using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Queries.BrowseByInterestsLocation;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models;

public class WhenMappingFromMediatorResultToBrowseByInterestsLocationApiResponse
{
    [Test, AutoData]
    public void Then_The_Response_Is_Mapped(BrowseByInterestsLocationQueryResult source)
    {
        var actual = (BrowseByInterestsLocationApiResponse)source;

        actual.Location.LocationName.Should().Be(source.LocationItem.Name);
        actual.Location.Lat.Should().Be(source.LocationItem.GeoPoint.First());
        actual.Location.Lon.Should().Be(source.LocationItem.GeoPoint.Last());
    }

    [Test]
    public void Then_If_Null_Then_Null_Returned()
    {
        var actual = (BrowseByInterestsLocationApiResponse)new BrowseByInterestsLocationQueryResult{LocationItem = null};

        actual.Location.Should().BeNull();
    }
}
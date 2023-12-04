using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Queries.BrowseByInterestsLocation;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchIndexLocation;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models;

public class WhenMappingFromMediatorResultToIndexLocationApiResponse
{
    [Test, AutoData]
    public void Then_The_Response_Is_Mapped(IndexLocationQueryResult source)
    {
        var actual = (IndexLocationApiResponse)source;

        actual.Location.LocationName.Should().Be(source.LocationItem.Name);
        actual.Location.Lat.Should().Be(source.LocationItem.GeoPoint.First());
        actual.Location.Lon.Should().Be(source.LocationItem.GeoPoint.Last());
    }

    [Test]
    public void Then_If_Null_Then_Null_Returned()
    {
        var actual = (IndexLocationApiResponse)new IndexLocationQueryResult{LocationItem = null};

        actual.Location.Should().BeNull();
    }
}
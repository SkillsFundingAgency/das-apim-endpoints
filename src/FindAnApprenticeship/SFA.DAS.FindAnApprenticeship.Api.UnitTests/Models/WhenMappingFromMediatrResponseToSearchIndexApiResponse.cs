using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchIndex;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models;

public class WhenMappingFromMediatrResponseToSearchIndexApiResponse
{
    [Test, AutoData]
    public void Then_The_Fields_Are_Mapped(SearchIndexQueryResult source)
    {
        var actual = (SearchIndexApiResponse)source;

        actual.Should().BeEquivalentTo(source, options=> options.Excluding(c=>c.LocationItem));
        actual.Location.LocationName.Should().Be(source.LocationItem.Name);
        actual.Location.Lat.Should().Be(source.LocationItem.GeoPoint.First());
        actual.Location.Lon.Should().Be(source.LocationItem.GeoPoint.Last());
        actual.SavedSearches.Should().BeEquivalentTo(source.SavedSearches);
        actual.Routes.Should().BeEquivalentTo(source.Routes.Select(c => (RouteApiResponse)c).ToList());
    }

    [Test, AutoData]
    public void Then_No_Routes_And_SavedSearches_Maps_Null(SearchIndexQueryResult source)
    {
        source.SavedSearches = null;
        source.Routes = null;
        
        var actual = (SearchIndexApiResponse)source;

        actual.Routes.Should().BeNull();
        actual.SavedSearches.Should().BeNull();
    }
}
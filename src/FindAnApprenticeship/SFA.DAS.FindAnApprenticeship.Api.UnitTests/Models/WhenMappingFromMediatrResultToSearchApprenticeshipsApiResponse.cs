using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models
{
    public class WhenMappingFromMediatrResultToSearchApprenticeshipsApiResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(SearchApprenticeshipsResult source)
        {
            var actual = (SearchApprenticeshipsApiResponse)source;
            
            actual.Should().BeEquivalentTo(source, options => options.Excluding(c=>c.LocationItem));
            actual.Location.LocationName.Should().Be(source.LocationItem.Name);
            actual.Location.Lat.Should().Be(source.LocationItem.GeoPoint.First());
            actual.Location.Lon.Should().Be(source.LocationItem.GeoPoint.Last());
        }
        
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped_For_Null_Location(SearchApprenticeshipsResult source)
        {
            source.LocationItem = null;
            
            var actual = (SearchApprenticeshipsApiResponse)source;
            
            actual.Should().BeEquivalentTo(source, options => options.Excluding(c=>c.LocationItem));
            actual.Location.Should().BeNull();
        }
    }
}
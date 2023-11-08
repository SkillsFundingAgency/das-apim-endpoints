using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetLocationsBySearch;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models
{
    public class WhenMappingMediatrResponseToGetLocationSearchResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetLocationsBySearchQueryResult source)
        {
            source.Locations = source.Locations.Select(l => { l.Postcode = null; return l; }).ToList();

            var actual = (GetLocationBySearchResponse)source;

            using (new AssertionScope())
            {
                actual.Locations.First().Should().BeEquivalentTo(source.Locations.First(), options => options.ExcludingMissingMembers());
                actual.Locations.Last().Should().BeEquivalentTo(source.Locations.Last(), options => options.ExcludingMissingMembers());
                actual.Locations.Count().Should().Be(source.Locations.Count());
            }
        }
    }
}
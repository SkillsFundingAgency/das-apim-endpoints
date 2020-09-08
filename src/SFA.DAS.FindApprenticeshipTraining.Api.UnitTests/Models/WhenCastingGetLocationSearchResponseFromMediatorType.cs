using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.Application.Locations.GetLocations;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Models
{
    public class WhenCastingGetLocationSearchResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetLocationsQueryResponse source)
        {
            var actual = (GetLocationSearchResponse) source;
            
            actual.Locations.ToList().First().Should().BeEquivalentTo(source.Locations.First());
            actual.Locations.ToList().Last().Should().BeEquivalentTo(source.Locations.Last());
            actual.Locations.Count().Should().Be(source.Locations.Count());
        }
    }
}
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Api.Models;
using SFA.DAS.EmployerDemand.Application.Locations.Queries.GetLocations;

namespace SFA.DAS.EmployerDemand.Api.UnitTests.Models
{
    public class WhenCastingGetLocationSearchResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetLocationsQueryResponse source)
        {
            //Arrange
            source.Locations = source.Locations.Select(c => { c.Postcode = null; return c; }).ToList();

            //Act
            var actual = (GetLocationSearchResponse) source;
            
            //Assert
            actual.Locations!.AsEnumerable().First().Should().BeEquivalentTo(source.Locations.First(), options=> options.ExcludingMissingMembers());
            actual.Locations!.AsEnumerable().Last().Should().BeEquivalentTo(source.Locations.Last(), options=> options.ExcludingMissingMembers());
            actual.Locations!.Count().Should().Be(source.Locations.Count());
        }
    }
}
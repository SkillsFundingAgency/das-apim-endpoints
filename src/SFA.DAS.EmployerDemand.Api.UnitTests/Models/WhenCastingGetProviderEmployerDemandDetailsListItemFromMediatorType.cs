using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Api.Models;
using SFA.DAS.EmployerDemand.InnerApi.Responses;

namespace SFA.DAS.EmployerDemand.Api.UnitTests.Models
{
    public class WhenCastingGetProviderEmployerDemandDetailsListItemFromMediatorType
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetEmployerCourseProviderDemandResponse source)
        {
            //Act
            var actual = (GetProviderEmployerDemandDetailsListItem) source;

            //Assert
            actual.Should().BeEquivalentTo(source, options => options.Excluding(c=>c.Location.LocationPoint));
            actual.Location.Location.GeoPoint.ToList().Should().BeEquivalentTo(source.Location.LocationPoint.GeoPoint);
        }
        
        [Test, AutoData]
        public void Then_Null_Returned_If_No_Location(GetEmployerCourseProviderDemandResponse source)
        {
            //Arrange
            source.Location = null;
            //Act
            var actual = (GetProviderEmployerDemandDetailsListItem) source;

            //Assert
            actual.Should().BeEquivalentTo(source, options => options.Excluding(c=>c.Location));
            actual.Location.Should().BeNull();
        }
    }
}
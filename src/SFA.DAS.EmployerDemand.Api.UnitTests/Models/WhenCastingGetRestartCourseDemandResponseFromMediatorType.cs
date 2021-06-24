using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Api.Models;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetRestartEmployerDemand;

namespace SFA.DAS.EmployerDemand.Api.UnitTests.Models
{
    public class WhenCastingGetRestartCourseDemandResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetRestartEmployerDemandQueryResult source)
        {
            //Act
            var actual = (GetRestartCourseDemandResponse)source;
            
            //Assert
            actual.RestartDemandExists.Should().Be(source.RestartDemandExists);
            actual.Should().BeEquivalentTo(source.EmployerDemand, options=> options
                .Excluding(c => c.StartSharingUrl)
                .Excluding(c => c.StopSharingUrl)
                .Excluding(c => c.Stopped)
                .Excluding(c => c.Course)
                .Excluding(c => c.Location)
                .Excluding(c => c.ContactEmailAddress)
                .Excluding(c => c.ExpiredCourseDemandId)
                .Excluding(c => c.LastStartDate)
            );
            actual.ContactEmail.Should().Be(source.EmployerDemand.ContactEmailAddress);
            actual.TrainingCourse.Should().BeEquivalentTo(source.EmployerDemand.Course);
            actual.Location.Location.GeoPoint.Should().BeEquivalentTo(source.EmployerDemand.Location.LocationPoint.GeoPoint);
            actual.Location.Name.Should().BeEquivalentTo(source.EmployerDemand.Location.Name);
            actual.TrainingCourse.LastStartDate.Should().Be(source.LastStartDate);
        }
    }
}
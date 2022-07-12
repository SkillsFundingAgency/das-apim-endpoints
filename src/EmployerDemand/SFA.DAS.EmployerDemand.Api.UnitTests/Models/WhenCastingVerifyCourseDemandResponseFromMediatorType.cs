using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Api.Models;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.VerifyEmployerDemand;

namespace SFA.DAS.EmployerDemand.Api.UnitTests.Models
{
    public class WhenCastingVerifyCourseDemandResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(VerifyEmployerDemandCommandResult source)
        {
            //Act
            var actual = (VerifyCourseDemandResponse) source;
            
            //Assert
            actual.Id.Should().Be(source.EmployerDemand.Id);
            actual.OrganisationName.Should().Be(source.EmployerDemand.OrganisationName);
            actual.ContactEmail.Should().Be(source.EmployerDemand.ContactEmailAddress);
            actual.EmailVerified.Should().Be(source.EmployerDemand.EmailVerified);
            actual.NumberOfApprentices.Should().Be(source.EmployerDemand.NumberOfApprentices);
            actual.Location.Name.Should().Be(source.EmployerDemand.Location.Name);
            actual.Location.Location.GeoPoint.Should().BeEquivalentTo(source.EmployerDemand.Location.LocationPoint.GeoPoint);
            actual.TrainingCourse.Id.Should().Be(source.EmployerDemand.Course.Id);
            actual.TrainingCourse.Title.Should().Be(source.EmployerDemand.Course.Title);
            actual.TrainingCourse.Level.Should().Be(source.EmployerDemand.Course.Level);
            actual.TrainingCourse.Route.Should().Be(source.EmployerDemand.Course.Route);
        }

        [Test]
        public void Then_If_Null_Then_Null_Returned()
        {
            //Arrange
            var response = new VerifyEmployerDemandCommandResult
            {
                EmployerDemand = null
            };
            
            //Act
            var actual = (VerifyCourseDemandResponse)response;
            
            //Assert
            actual.Should().BeNull();
        }
    }
}
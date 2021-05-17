using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Api.Models;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.VerifyEmployerDemand;

namespace SFA.DAS.EmployerDemand.Api.UnitTests.Models
{
    public class WhenCastingGetCourseListItemFromVerifyEmployerDemandMediatorType
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(VerifyEmployerDemandCommandResult source)
        {
            //Act
            var actual = (GetCourseListItem) source;
            
            //Assert
            actual.Id.Should().Be(source.EmployerDemand.CourseId);
            actual.Title.Should().Be(source.EmployerDemand.CourseTitle);
            actual.Level.Should().Be(source.EmployerDemand.CourseLevel);
            actual.Sector.Should().Be(source.EmployerDemand.CourseRoute);
        }
    }
}
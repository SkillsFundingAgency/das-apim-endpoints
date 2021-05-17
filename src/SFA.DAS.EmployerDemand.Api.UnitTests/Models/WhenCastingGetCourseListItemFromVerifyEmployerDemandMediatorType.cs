using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Api.Models;
using SFA.DAS.EmployerDemand.InnerApi.Responses;

namespace SFA.DAS.EmployerDemand.Api.UnitTests.Models
{
    public class WhenCastingGetCourseListItemFromVerifyEmployerDemandMediatorType
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(EmployerDemandCourse source)
        {
            //Act
            var actual = (GetCourseListItem) source;
            
            //Assert
            actual.Should().BeEquivalentTo(source);
        }
    }
}
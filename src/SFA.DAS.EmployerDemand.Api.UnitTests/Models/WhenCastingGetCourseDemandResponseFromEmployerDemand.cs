using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Api.Models;
using SFA.DAS.EmployerDemand.InnerApi.Responses;

namespace SFA.DAS.EmployerDemand.Api.UnitTests.Models
{
    public class WhenCastingGetCourseDemandResponseFromEmployerDemand
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetEmployerDemandResponse source)
        {
            //Act
            var actual = (GetCourseDemandResponse) source;
            
            //Assert
            actual.Should().BeEquivalentTo(source, options => options
                .Excluding(c=>c.Location)
                .Excluding(c=>c.Course)
                .Excluding(c=>c.ContactEmailAddress)
                .Excluding(c=>c.StopSharingUrl)
                .Excluding(c=>c.StartSharingUrl)
                .Excluding(c=>c.Stopped)
                .Excluding(c=>c.ExpiredCourseDemandId)
                .Excluding(c => c.LastStartDate)
            );
            actual.ContactEmail.Should().Be(source.ContactEmailAddress);
        }

        [Test]
        public void Then_If_Null_Then_Null_Returned()
        {
            //Act
            var actual = (GetCourseDemandResponse) (GetEmployerDemandResponse) null;
            
            //Assert
            actual.Should().BeNull();
        }
    }
}
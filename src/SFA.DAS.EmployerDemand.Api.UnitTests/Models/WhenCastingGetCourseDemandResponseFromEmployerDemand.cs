using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Api.Models;

namespace SFA.DAS.EmployerDemand.Api.UnitTests.Models
{
    public class WhenCastingGetCourseDemandResponseFromEmployerDemand
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(Domain.Models.EmployerDemand source)
        {
            //Act
            var actual = (GetCourseDemandResponse) source;
            
            //Assert
            actual.Should().BeEquivalentTo(source, options => options
                .Excluding(c=>c.Location)
                .Excluding(c=>c.Standard)
            );
        }

        [Test]
        public void Then_If_Null_Then_Null_Returned()
        {
            //Act
            var actual = (GetCourseDemandResponse) (Domain.Models.EmployerDemand) null;
            
            //Assert
            actual.Should().BeNull();
        }
    }
}
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Api.Models;
using SFA.DAS.EmployerDemand.Domain.Models;

namespace SFA.DAS.EmployerDemand.Api.UnitTests.Models
{
    public class WhenCastingFromStandardDatesToCourseDates
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped_Correctly(StandardDates source)
        {
            //Act
            var actual = (CourseDates) source;

            //Assert
            actual.EffectiveTo.Should().Be(source.EffectiveTo);
            actual.EffectiveFrom.Should().Be(source.EffectiveFrom);
            actual.LastDateStarts.Should().Be(source.LastDateStarts);
        }
    }
}
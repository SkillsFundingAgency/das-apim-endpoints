using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.Models;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Models
{
    public class WhenMappingToApprenticeshipDtoFromMediatorResult
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(ApprenticeshipItem source)
        {
            var actual = (ApprenticeshipDto) source;
            
            actual.Should().BeEquivalentTo(source, options=>options.Excluding(c=>c.StartDate));
        }
    }
}
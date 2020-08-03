using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.Models;
using SFA.DAS.EmployerIncentives.Models.Commitments;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Models
{
    public class WhenMappingToEligibleApprenticeshipDtoFromMediatorResult
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(ApprenticeshipItem source)
        {
            var actual = (EligibleApprenticeshipDto) source;
            
            actual.Should().BeEquivalentTo(source, options=>options.Excluding(c=>c.StartDate));
        }
    }
}
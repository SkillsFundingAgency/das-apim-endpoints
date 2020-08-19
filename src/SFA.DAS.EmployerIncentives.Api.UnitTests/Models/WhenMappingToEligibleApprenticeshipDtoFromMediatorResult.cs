using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Commitments;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Models
{
    public class WhenMappingToEligibleApprenticeshipDtoFromMediatorResult
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(ApprenticeshipItem source)
        {
            var actual = (EligibleApprenticeshipDto) source;
            
            actual.ApprenticeshipId.Should().Be(source.Id);
            actual.CourseName.Should().Be(source.CourseName);
            actual.FirstName.Should().Be(source.FirstName);
            actual.LastName.Should().Be(source.LastName);
            actual.Uln.Should().Be(source.Uln);
        }
    }
}
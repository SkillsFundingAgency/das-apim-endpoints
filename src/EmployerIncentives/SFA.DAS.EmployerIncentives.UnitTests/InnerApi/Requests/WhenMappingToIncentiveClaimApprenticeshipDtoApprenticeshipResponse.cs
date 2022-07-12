using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Commitments;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenMappingToIncentiveClaimApprenticeshipDtoApprenticeshipResponse
    {
        [TestCase(ApprenticeshipEmployerType.Levy)]
        [TestCase(ApprenticeshipEmployerType.NonLevy)]
        [TestCase(null)]
        public void Then_The_Fields_Are_Correctly_Mapped(ApprenticeshipEmployerType? levyStatus)
        {
            var f = new Fixture();

            var source = f.Build<ApprenticeshipResponse>()
                .With(x => x.ApprenticeshipEmployerTypeOnApproval, levyStatus)
                .Create();

            var actual = (IncentiveClaimApprenticeshipDto) source;
            
            actual.ApprenticeshipId.Should().Be(source.Id);
            actual.FirstName.Should().Be(source.FirstName);
            actual.LastName.Should().Be(source.LastName);
            actual.DateOfBirth.Should().Be(source.DateOfBirth);
            actual.Uln.Should().Be(source.Uln);
            actual.PlannedStartDate.Should().Be(source.StartDate);
        }
    }
}
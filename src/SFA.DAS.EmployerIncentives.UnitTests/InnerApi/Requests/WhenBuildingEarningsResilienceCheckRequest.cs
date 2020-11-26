using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.EarningsResilienceCheck;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingEarningsResilienceCheckRequest
    {
        [Test]
        public void Then_The_PostUrl_Is_Correctly_Built()
        {
            var actual = new EarningsResilenceCheckRequest();

            actual.PostUrl.Should().Be("earnings-resilience-check");
        }
    }
}
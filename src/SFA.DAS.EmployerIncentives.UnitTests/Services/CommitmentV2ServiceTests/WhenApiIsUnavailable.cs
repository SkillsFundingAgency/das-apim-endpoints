using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Services;

namespace SFA.DAS.EmployerIncentives.UnitTests.Services.CommitmentV2ServiceTests
{
    [TestFixture] 
    public class WhenApiIsUnavailable
    {
        [Test]
        public async Task Then_health_check_should_return_false()
        {
            var sut = new CommitmentsV2Service(null);

            var result = await sut.IsHealthy();
            result.Should().Be(false);
        }
    }
}

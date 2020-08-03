using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.EmployerIncentivesService
{
    [TestFixture] 
    public class WhenApiIsUnavailable
    {
        [Test]
        public async Task Then_health_check_should_return_false()
        {
            var sut = new EmployerIncentives.Application.Services.EmployerIncentivesService(null);

            var result = await sut.IsHealthy();
            result.Should().Be(false);
        }
    }
}

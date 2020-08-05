using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.EmployerIncentivesService
{
    [TestFixture] 
    public class WhenApiIsUnavailable
    {
        [Test]
        public async Task Then_Health_Check_Should_Return_False()
        {
            var sut = new EmployerIncentives.Application.Services.EmployerIncentivesService(null);

            var result = await sut.IsHealthy();
            result.Should().Be(false);
        }
    }
}

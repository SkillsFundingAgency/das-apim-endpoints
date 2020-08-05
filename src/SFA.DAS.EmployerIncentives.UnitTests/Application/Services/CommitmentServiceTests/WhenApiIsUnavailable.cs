using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.CommitmentServiceTests
{
    [TestFixture] 
    public class WhenApiIsUnavailable
    {
        [Test]
        public async Task Then_Health_Check_Should_Return_False()
        {
            var sut = new CommitmentsService(null);

            var result = await sut.IsHealthy();
            result.Should().Be(false);
        }
    }
}

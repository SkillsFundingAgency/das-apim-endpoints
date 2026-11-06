using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.EmployerFinanceJobs.Api.Controllers;

namespace SFA.DAS.EmployerFinanceJobs.UnitTests.Controllers;

[TestFixture]
internal class PingControllerTests
{
    [Test]
    public void Ping_Returns_Pong()
    {
        // Act
        var result = new PingController().Ping();

        // Assert
        var okResult = result.Should().BeOfType<Ok<string>>().Subject;
        okResult.Value.Should().Be("Pong");
    }
}

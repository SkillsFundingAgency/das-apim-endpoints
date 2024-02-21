using AutoFixture.NUnit3;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using SFA.DAS.EmployerAan.Api.HealthCheck;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.Api.UnitTests.HealthCheck;

public class AanHubApiHealthChecksTests : HealthChecksTestsBase
{
    [Test, MoqAutoData]
    public async Task CheckHealthAsync_ValidQueryResult_ReturnsHealthyStatus(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        HealthCheckContext healthCheckContext,
        AanHubApiHealthCheck healthCheck)
    {
        apiClient.Setup(x => x.GetHealth(It.IsAny<CancellationToken>())).ReturnsAsync(ResponseMessageOk);

        var actual = await healthCheck.CheckHealthAsync(healthCheckContext, CancellationToken.None);

        Assert.That(actual.Status, Is.EqualTo(HealthStatus.Healthy));
    }

    [Test, MoqAutoData]
    public async Task CheckHealthAsync_NotValidQueryResult_ReturnsUnHealthyStatus(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        HealthCheckContext healthCheckContext,
        AanHubApiHealthCheck healthCheck)
    {
        apiClient.Setup(x => x.GetHealth(It.IsAny<CancellationToken>())).ReturnsAsync(ResponseMessageBadRequest);

        var actual = await healthCheck.CheckHealthAsync(healthCheckContext, CancellationToken.None);

        Assert.That(actual.Status, Is.EqualTo(HealthStatus.Unhealthy));
    }
}

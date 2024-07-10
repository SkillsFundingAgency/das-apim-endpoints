using AutoFixture.NUnit3;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using SFA.DAS.RoatpOversight.Api.HealthCheck;
using SFA.DAS.RoatpOversight.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RoatpOversight.Api.UnitTests.HealthCheck;
public class AanHubApiHealthChecksTests : HealthChecksTestsBase
{
    [Test, MoqAutoData]
    public async Task CheckHealthAsync_ValidQueryResult_ReturnsHealthyStatus(
        [Frozen] Mock<IRoatpV2ApiClient> apiClient,
        HealthCheckContext healthCheckContext,
        RoatpV2ApiHealthCheck healthCheck,
        CancellationToken cancellationToken)
    {
        apiClient.Setup(x => x.GetHealth(cancellationToken)).ReturnsAsync(ResponseMessageOk);

        var actual = await healthCheck.CheckHealthAsync(healthCheckContext, cancellationToken);

        Assert.That(actual.Status, Is.EqualTo(HealthStatus.Healthy));
    }

    [Test, MoqAutoData]
    public async Task CheckHealthAsync_NotValidQueryResult_ReturnsUnHealthyStatus(
        [Frozen] Mock<IRoatpV2ApiClient> apiClient,
        HealthCheckContext healthCheckContext,
        RoatpV2ApiHealthCheck healthCheck)
    {
        apiClient.Setup(x => x.GetHealth(It.IsAny<CancellationToken>())).ReturnsAsync(ResponseMessageBadRequest);

        var actual = await healthCheck.CheckHealthAsync(healthCheckContext, CancellationToken.None);

        Assert.That(actual.Status, Is.EqualTo(HealthStatus.Unhealthy));
    }
}
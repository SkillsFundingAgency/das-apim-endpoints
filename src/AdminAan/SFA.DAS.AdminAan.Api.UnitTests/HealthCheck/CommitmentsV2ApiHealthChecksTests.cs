using AutoFixture.NUnit3;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using SFA.DAS.AdminAan.Api.HealthCheck;
using SFA.DAS.AdminAan.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminAan.Api.UnitTests.HealthCheck;

public class CommitmentsV2ApiHealthChecksTests : HealthChecksTestsBase
{
    [Test, MoqAutoData]
    public async Task CheckHealthAsync_ValidQueryResult_ReturnsHealthyStatus(
        [Frozen] Mock<ICommitmentsV2ApiClient> apiClient,
        HealthCheckContext healthCheckContext,
        CommitmentsV2InnerApiHealthCheck healthCheck,
        CancellationToken cancellationToken)
    {
        apiClient.Setup(x => x.GetHealth(cancellationToken)).ReturnsAsync(ResponseMessageOk);

        var actual = await healthCheck.CheckHealthAsync(healthCheckContext, cancellationToken);

        Assert.That(actual.Status, Is.EqualTo(HealthStatus.Healthy));
    }

    [Test, MoqAutoData]
    public async Task CheckHealthAsync_NotValidQueryResult_ReturnsUnHealthyStatus(
        [Frozen] Mock<ICommitmentsV2ApiClient> apiClient,
        HealthCheckContext healthCheckContext,
        CommitmentsV2InnerApiHealthCheck healthCheck)
    {
        apiClient.Setup(x => x.GetHealth(It.IsAny<CancellationToken>())).ReturnsAsync(ResponseMessageBadRequest);

        var actual = await healthCheck.CheckHealthAsync(healthCheckContext, CancellationToken.None);

        Assert.That(actual.Status, Is.EqualTo(HealthStatus.Unhealthy));
    }
}
using AutoFixture.NUnit3;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Roatp.Api.HealthCheck;
using SFA.DAS.Roatp.Infrastructure;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Roatp.Api.UnitTests.HealthCheck;

public class CharitiesApiHealthCheckTests : HealthChecksTestsBase
{
    [Test, MoqAutoData]
    public async Task CheckHealthAsync_ValidQueryResult_ReturnsHealthyStatus(
        [Frozen] Mock<ICharitiesRestApiClient> apiClient,
        HealthCheckContext healthCheckContext,
        CharitiesApiHealthCheck healthCheck,
        CancellationToken cancellationToken)
    {
        apiClient.Setup(x => x.GetHealth(cancellationToken)).ReturnsAsync(ResponseMessageOk);

        var actual = await healthCheck.CheckHealthAsync(healthCheckContext, cancellationToken);

        Assert.That(actual.Status, Is.EqualTo(HealthStatus.Healthy));
    }

    [Test, MoqAutoData]
    public async Task CheckHealthAsync_NotValidQueryResult_ReturnsUnHealthyStatus(
        [Frozen] Mock<ICharitiesRestApiClient> apiClient,
        HealthCheckContext healthCheckContext,
        CharitiesApiHealthCheck healthCheck,
        CancellationToken cancellationToken)
    {
        apiClient.Setup(x => x.GetHealth(cancellationToken)).ReturnsAsync(ResponseMessageBadRequest);

        var actual = await healthCheck.CheckHealthAsync(healthCheckContext, cancellationToken);

        Assert.That(actual.Status, Is.EqualTo(HealthStatus.Unhealthy));
    }
}
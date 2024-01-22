using AutoFixture.NUnit3;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using SFA.DAS.ApprenticeAan.Api.HealthCheck;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Api.UnitTests.HealthCheck;
public class ApprenticeAanInnerApiHealthChecksTests
{
    [Test, MoqAutoData]
    public async Task CheckHealthAsync_ValidQueryResult_ReturnsHealthyStatus(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        HealthCheckContext healthCheckContext,
        List<Calendar> calendars,
        ApprenticeAanInnerApiHealthCheck healthCheck)
    {
        apiClient.Setup(x => x.GetCalendars(It.IsAny<CancellationToken>())).ReturnsAsync(calendars);

        var actual = await healthCheck.CheckHealthAsync(healthCheckContext, CancellationToken.None);

        Assert.That(actual.Status, Is.EqualTo(HealthStatus.Healthy));
    }

    [Test, MoqAutoData]
    public async Task CheckHealthAsync_NotValidQueryResult_ReturnsUnHealthyStatus(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        HealthCheckContext healthCheckContext,
        List<Calendar> calendars,
        ApprenticeAanInnerApiHealthCheck healthCheck)
    {
        apiClient.Setup(x => x.GetCalendars(It.IsAny<CancellationToken>())).ReturnsAsync(calendars);

        var actual = await healthCheck.CheckHealthAsync(healthCheckContext, CancellationToken.None);

        Assert.That(actual.Status, Is.EqualTo(HealthStatus.Healthy));
    }

    [Test, MoqAutoData]
    public async Task CheckHealthAsync_ExceptionThrown_ReturnsUnHealthyStatus(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        HealthCheckContext healthCheckContext,
        ApprenticeAanInnerApiHealthCheck healthCheck)
    {
        apiClient.Setup(x => x.GetCalendars(It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidOperationException());

        var actual = await healthCheck.CheckHealthAsync(healthCheckContext, CancellationToken.None);

        Assert.That(actual.Status, Is.EqualTo(HealthStatus.Unhealthy));
    }
}

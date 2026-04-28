using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.Apim.Shared.InnerApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Net;
using System.Threading;

namespace SFA.DAS.SharedOuterApi.UnitTests.Infrastructure.HealthCheck;

public class WhenCheckingHealthOnApprenticeAccountsApiClient
{
    [Test, MoqAutoData]
    public async Task Then_The_Ping_Endpoint_Is_Called(
        [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> client,
        HealthCheckContext healthCheckContext,
        ApprenticeAccountsApiHealthCheck healthCheck)
    {
        // Act
        await healthCheck.CheckHealthAsync(healthCheckContext, CancellationToken.None);

        // Assert
        client.Verify(x => x.GetResponseCode(It.IsAny<GetPingRequest>()), Times.Once);
    }

    [Test]
    [MoqInlineAutoData(HttpStatusCode.OK, HealthStatus.Healthy)]
    [MoqInlineAutoData(HttpStatusCode.NotFound, HealthStatus.Unhealthy)]
    [MoqInlineAutoData(HttpStatusCode.InternalServerError, HealthStatus.Unhealthy)]
    public async Task Then_The_Correct_HealthStatus_Is_Returned(
        HttpStatusCode httpStatusCode,
        HealthStatus healthStatus,
        [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> client,
        HealthCheckContext healthCheckContext,
        ApprenticeAccountsApiHealthCheck healthCheck)
    {
        // Arrange
        client.Setup(x => x.GetResponseCode(It.IsAny<GetPingRequest>()))
            .ReturnsAsync(httpStatusCode);

        // Act
        var actual = await healthCheck.CheckHealthAsync(healthCheckContext, CancellationToken.None);

        // Assert
        Assert.That(healthStatus, Is.EqualTo(actual.Status));
    }

    [Test, MoqAutoData]
    public void Then_HealthCheckResultDescription_IsConsistent()
    {
        ApprenticeAccountsApiHealthCheck.HealthCheckResultDescription.Should().Be(ApprenticeAccountsApiHealthCheck.HealthCheckDescription + " check");
    }
}
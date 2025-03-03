using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using SFA.DAS.AdminAan.Api.HealthCheck;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminAan.Api.UnitTests.HealthCheck;

public class EducationalOrganisationApiHealthChecksTests : HealthChecksTestsBase
{
    [Test, MoqAutoData]
    public async Task CheckHealthAsync_ValidQueryResult_ReturnsHealthyStatus(
        [Frozen] Mock<IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration>> apiClient,
        HealthCheckContext healthCheckContext,
        EducationalOrganisationApiHealthCheck healthCheck,
        CancellationToken cancellationToken)
    {
        // Arrange
        apiClient
            .Setup(x => x.GetResponseCode(It.IsAny<GetPingRequest>()))
            .ReturnsAsync(HttpStatusCode.OK);

        // Act
        var actual = await healthCheck.CheckHealthAsync(healthCheckContext, cancellationToken);

        // Assert
        actual.Status.Should().Be(HealthStatus.Healthy);
    }

    [Test, MoqAutoData]
    public async Task CheckHealthAsync_NotValidQueryResult_ReturnsUnHealthyStatus(
        [Frozen] Mock<IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration>> apiClient,
        HealthCheckContext healthCheckContext,
        EducationalOrganisationApiHealthCheck healthCheck)
    {
        apiClient.Setup(x => x.GetResponseCode(It.IsAny<GetPingRequest>()))
            .ReturnsAsync(HttpStatusCode.BadRequest);

        var actual = await healthCheck.CheckHealthAsync(healthCheckContext, CancellationToken.None);

        Assert.That(actual.Status, Is.EqualTo(HealthStatus.Unhealthy));
    }
}
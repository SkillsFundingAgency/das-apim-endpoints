﻿using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.SharedOuterApi.UnitTests.Infrastructure.HealthCheck;

public class WhenCheckingHealthOnPensionRegulatorApi
{
    [Test]
    [MoqInlineAutoData(HttpStatusCode.OK, HealthStatus.Healthy)]
    [MoqInlineAutoData(HttpStatusCode.NotFound, HealthStatus.Unhealthy)]
    [MoqInlineAutoData(HttpStatusCode.InternalServerError, HealthStatus.Unhealthy)]
    public async Task Then_The_Correct_HealthStatus_Is_Returned(
        HttpStatusCode httpStatusCode,
        HealthStatus healthStatus,
        [Frozen] Mock<IProviderCoursesApiClient<PensionRegulatorApiConfiguration>> client,
        HealthCheckContext healthCheckContext,
        PensionsRegulatorApiHealthCheck healthCheck)
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
        PensionsRegulatorApiHealthCheck.HealthCheckResultDescription.Should().Be(PensionsRegulatorApiHealthCheck.HealthCheckDescription + " check");
    }
}
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.SharedOuterApi.UnitTests.Infrastructure.HealthCheck
{
    public class WhenCheckingHealthOnLocationApi
    {
        [Test, MoqAutoData]
        public async Task Then_The_Ping_Endpoint_Is_Called(
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> client,
            HealthCheckContext healthCheckContext,
            LocationsApiHealthCheck healthCheck)
        {
            //Act
            await healthCheck.CheckHealthAsync(healthCheckContext, CancellationToken.None);
            //Assert
            client.Verify(x => x.GetResponseCode(It.IsAny<GetPingRequest>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_If_It_Is_Successful_Returns_Healthy(
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> client,
            HealthCheckContext healthCheckContext,
            LocationsApiHealthCheck healthCheck)
        {
            //Arrange
            client.Setup(x => x.GetResponseCode(It.IsAny<GetPingRequest>()))
                .ReturnsAsync(HttpStatusCode.OK);
            //Act
            var actual = await healthCheck.CheckHealthAsync(healthCheckContext, CancellationToken.None);
            //Assert
            Assert.That(HealthStatus.Healthy, Is.EqualTo(actual.Status));
        }

        [Test, MoqAutoData]
        public async Task And_Not_Successful_Returns_UnHealthy(
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> client,
            HealthCheckContext healthCheckContext,
            LocationsApiHealthCheck healthCheck)
        {
            //Arrange
            client.Setup(x => x.GetResponseCode(new GetPingRequest()))
                .ReturnsAsync(HttpStatusCode.NotFound);
            //Act
            var actual = await healthCheck.CheckHealthAsync(healthCheckContext, CancellationToken.None);
            //Assert
            Assert.That(HealthStatus.Unhealthy, Is.EqualTo(actual.Status));
        }
    }
}
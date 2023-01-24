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
    public class WhenCallingPingOnTheCourseManagementApiClient
    {
        [Test, MoqAutoData]
        public async Task Then_The_Ping_Endpoint_Is_Called_For_CourseManagementApi(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> client,
            HealthCheckContext healthCheckContext,
            CourseManagementApiHealthCheck healthCheck)
        {
            //Act
            await healthCheck.CheckHealthAsync(healthCheckContext, CancellationToken.None);
            //Assert
            client.Verify(x => x.GetResponseCode(It.IsAny<GetPingRequest>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_If_It_Is_Successful_200_Is_Returned(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> client,
            HealthCheckContext healthCheckContext,
            CourseManagementApiHealthCheck healthCheck)
        {
            //Arrange
            client.Setup(x => x.GetResponseCode(It.IsAny<GetPingRequest>()))
                .ReturnsAsync(HttpStatusCode.OK);
            //Act
            var actual = await healthCheck.CheckHealthAsync(healthCheckContext, CancellationToken.None);
            //Assert
            Assert.AreEqual(HealthStatus.Healthy, actual.Status);
        }

        [Test, MoqAutoData]
        public async Task And_CourseDeliveryApi_Ping_Not_Found_Then_Unhealthy(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> client,
            HealthCheckContext healthCheckContext,
            CourseManagementApiHealthCheck healthCheck)
        {
            //Arrange
            client.Setup(x => x.GetResponseCode(new GetPingRequest()))
                .ReturnsAsync(HttpStatusCode.NotFound);
            //Act
            var actual = await healthCheck.CheckHealthAsync(healthCheckContext, CancellationToken.None);
            //Assert
            Assert.AreEqual(HealthStatus.Unhealthy, actual.Status);
        }
    }
}
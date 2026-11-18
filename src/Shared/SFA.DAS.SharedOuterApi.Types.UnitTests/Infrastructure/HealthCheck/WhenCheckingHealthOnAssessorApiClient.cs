using System.Net;
using System.Threading;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.InnerApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.SharedOuterApi.UnitTests.Infrastructure.HealthCheck
{
    public class WhenCheckingHealthOnAssessorApiClient
    {
        private Mock<IAssessorApiClient<AssessorApiConfiguration>> _apiClient;
        private AssessorApiHealthCheck _sut;

        [SetUp]
        public void SetUp()
        {
            _apiClient =
                new Mock<IAssessorApiClient<AssessorApiConfiguration>>();

            _sut = new AssessorApiHealthCheck(
                _apiClient.Object,
                Mock.Of<ILogger<AssessorApiHealthCheck>>());
        }

        [TestCase(HttpStatusCode.OK, HealthStatus.Healthy)]
        [TestCase(HttpStatusCode.NotFound, HealthStatus.Unhealthy)]
        [TestCase(HttpStatusCode.InternalServerError, HealthStatus.Unhealthy)]
        [TestCase(HttpStatusCode.ServiceUnavailable, HealthStatus.Unhealthy)]
        public async Task Then_The_Inner_Api_Is_Pinged_And_Health_Is_Returned(
            HttpStatusCode statusCode,
            HealthStatus expectedHealth)
        {
            _apiClient
                .Setup(x => x.GetResponseCode(
                    It.IsAny<GetPingRequest>()))
                .ReturnsAsync(statusCode);

            var actual = await _sut.CheckHealthAsync(
                new HealthCheckContext(),
                CancellationToken.None);

            actual.Status.Should().Be(expectedHealth);

            actual.Description.Should().Be(
                AssessorApiHealthCheck.HealthCheckResultDescription);

            _apiClient.Verify(
                x => x.GetResponseCode(
                    It.IsAny<GetPingRequest>()),
                Times.Once);
        }
    }
}
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Services;

namespace SFA.DAS.EmployerIncentives.UnitTests.Services.CommitmentsV2ServiceTests
{
    [TestFixture]
    public class WhenGettingCommitmentsV2HealthCheckResult
    {
        private HttpClient _httpClient;
        private Mock<HttpClientHandler> _httpClientHandlerMock;
        public HttpResponseMessage _httpResponseMessage;
        private CommitmentsV2Service _sut;
        string _baseUrl = "http://www.commitmentsv2.com";

        [SetUp]
        public void Arrange()
        {
            _httpClientHandlerMock = new Mock<HttpClientHandler>();

            _httpClient = new HttpClient(_httpClientHandlerMock.Object);
            _httpClient.BaseAddress = new Uri(_baseUrl);

            _sut = new CommitmentsV2Service(_httpClient);
        }

        [TestCase("Healthy", HealthStatus.Healthy)]
        [TestCase("Degraded", HealthStatus.Degraded)]
        [TestCase("Unhealthy", HealthStatus.Unhealthy)]
        [TestCase("Unknown", HealthStatus.Unhealthy)]
        [TestCase("", HealthStatus.Unhealthy)]
        public async Task Then_It_Returns_The_Status(string innerApiStatus, HealthStatus expected)
        {
            SetupHealthResponseFromInnerApi(innerApiStatus);

            var result = await _sut.HealthCheck();
            
            result.Status.Should().Be(expected);
        }

        [Test]
        public async Task When_Inner_Api_Is_Unavailable_Then_HealthCheck_return_Status_As_Unhealthy()
        {
            var result = await _sut.HealthCheck();

            result.Status.Should().Be(HealthStatus.Unhealthy);
        }

        private void SetupHealthResponseFromInnerApi(string status)
        {
            _httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"Status\" : \"" + status + "\"}", Encoding.UTF8, "application/json") };
            _httpClientHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(x => true),
                    ItExpr.IsAny<CancellationToken>()).ReturnsAsync(_httpResponseMessage);
        }
    }
}

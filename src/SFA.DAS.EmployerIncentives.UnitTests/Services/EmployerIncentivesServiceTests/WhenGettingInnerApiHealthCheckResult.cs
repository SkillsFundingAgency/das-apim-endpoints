using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Services;

namespace SFA.DAS.EmployerIncentives.UnitTests.Services.EmployerIncentivesServiceTests
{
    [TestFixture]
    public class WhenGettingInnerApiHealthCheckResult
    {
        private HttpClient _httpClient;
        private Mock<HttpClientHandler> _httpClientHandlerMock;
        public HttpResponseMessage _httpResponseMessage;
        private EmployerIncentivesService _sut;
        string _baseUrl = "http://www.innerApi.com/";

        [SetUp]
        public void Arrange()
        {
            _httpClientHandlerMock = new Mock<HttpClientHandler>();

            _httpClient = new HttpClient(_httpClientHandlerMock.Object);
            _httpClient.BaseAddress = new Uri(_baseUrl);

            _sut = new EmployerIncentivesService(_httpClient);
        }

        [Test]
        public async Task Then_HealthCheck_Calls_Api_Correctly()
        {
            SetupHealthResponseFromInnerApi("Healthy");

            await _sut.HealthCheck();
            
            VerifyMethodAndPath(HttpMethod.Get, $"health");
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
            _httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(status) };
            _httpClientHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(x => true),
                    ItExpr.IsAny<CancellationToken>()).ReturnsAsync(_httpResponseMessage);
        }

        private void VerifyMethodAndPath(HttpMethod method, string path)
        {
            var expectedUri = new Uri(new Uri(_baseUrl), path);

            _httpClientHandlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(x => x.Method == method && x.RequestUri == expectedUri), 
                ItExpr.IsAny<CancellationToken>());
        }
    }
}

using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Infrastructure.Api;
using SFA.DAS.EmployerIncentives.Services;

namespace SFA.DAS.EmployerIncentives.UnitTests.Services.ApiPassThroughServiceTests
{
    [TestFixture]
    public class WhenDeletingResource
    {
        private HttpClient _httpClient;
        private Mock<HttpClientHandler> _httpClientHandlerMock;

        private Mock<ILoggerFactory> _loggerFactoryMock;
        private Mock<ILogger<PassThroughApiClient>> _loggerMock;
        public HttpResponseMessage _httpResponseMessage;
        private ApiPassThroughService _sut;
        string _baseUrl = "http://www.test.com/";

        [SetUp]
        public void Arrange()
        { 
            _httpClientHandlerMock = new Mock<HttpClientHandler>();
            _loggerMock = new Mock<ILogger<PassThroughApiClient>>();
            _loggerFactoryMock = new Mock<ILoggerFactory>();
            _loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(_loggerMock.Object);

            _httpClient = new HttpClient(_httpClientHandlerMock.Object); 
            _httpClient.BaseAddress = new Uri(_baseUrl);

            _sut = new ApiPassThroughService(_httpClient, _loggerFactoryMock.Object);
        }

        [Test]
        public async Task When_Deleting_A_Resource_It_Calls_Api_Correctly()
        {
            SetupNoJsonResponseFromInnerApi();

            await _sut.DeleteAsync("test/123");

            VerifyMethodAndPath(HttpMethod.Delete, $"test/123");
        }
        private void SetupNoJsonResponseFromInnerApi()
        {
            _httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NoContent);
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

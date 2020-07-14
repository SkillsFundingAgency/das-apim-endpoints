using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Infrastructure.Api;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.Services.PassThroughApiClientTests
{
    [TestFixture]
    public class WhenPostingJson
    {
        private HttpClient _httpClient;
        private Mock<HttpClientHandler> _httpClientHandlerMock;
        private Mock<ILogger<PassThroughApiClient>> _loggerMock;
        public HttpResponseMessage _httpResponseMessage;
        private PassThroughApiClient _sut;
        string _baseUrl = "http://www.test.com/";

        [SetUp]
        public void Arrange()
        {
            _loggerMock = new Mock<ILogger<PassThroughApiClient>>();
            _httpClientHandlerMock = new Mock<HttpClientHandler>();

            _httpClient = new HttpClient(_httpClientHandlerMock.Object);
            _httpClient.BaseAddress = new Uri(_baseUrl);

            _sut = new PassThroughApiClient(_httpClient, _loggerMock.Object);
        }

        [Test, MoqAutoData]
        public async Task When_Posting_To_An_EndPoint_It_Calls_Api_Correctly(TestRequest request)
        {
            SetupJsonResponseFromInnerApi();

            await _sut.Post("test/123", request);
            VerifyMethodAndPath(HttpMethod.Post, $"test/123");
        }

        [Test, MoqAutoData]
        public async Task When_Posting_To_An_EndPoint_It_Returns_Json_Value(TestRequest request)
        {
            SetupJsonResponseFromInnerApi();

            var result = await _sut.Post("test/123", request);

            VerifyApiResponseIsReturned(result);
        }

        [Test]
        public async Task When_Posting_To_An_EndPoint_Which_Returns_No_Json_Value()
        {
            SetupNoJsonResponseFromInnerApi();

            var result = await _sut.Get("test/123");

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(_httpResponseMessage.StatusCode);
            result.Json.Should().BeNull();
        }


        private void SetupJsonResponseFromInnerApi()
        {
            _httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"Test\" : \"XXXX\"}", Encoding.UTF8, "application/json") };
            _httpClientHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(x => true),
                    ItExpr.IsAny<CancellationToken>()).ReturnsAsync(_httpResponseMessage);
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

        private void VerifyApiResponseIsReturned(InnerApiResponse result)
        {
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(_httpResponseMessage.StatusCode);
            result.Json.Should().NotBeNull();
            result.Json.RootElement.GetProperty("Test").GetString().Should().Be("XXXX");
        }

        public class TestRequest
        {
            public string TestValue { get; set; }
        }
    }
}

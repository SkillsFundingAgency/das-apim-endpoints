﻿using System;
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

namespace SFA.DAS.EmployerIncentives.UnitTests.Services.PassThroughApiClientTests
{
    [TestFixture]
    public class WhenGettingJsonResult
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

        [Test]
        public async Task When_Querying_An_EndPoint_It_Calls_Api_Correctly()
        {
            SetupJsonResponseFromInnerApi();

            await _sut.Get("test/123");
            VerifyMethodAndPath(HttpMethod.Get, $"test/123");
        }

        [Test]
        public async Task When_Querying_An_EndPoint_It_Returns_Json_Value()
        {
            SetupJsonResponseFromInnerApi();

            var result = await _sut.Get("test/123");
            
            VerifyApiResponseIsReturned(result);
        }

        [Test]
        public async Task When_Querying_An_EndPoint_Which_Returns_No_Json_Value()
        {
            SetupNoJsonResponseFromInnerApi();

            var result = await _sut.Get("test/123");

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(_httpResponseMessage.StatusCode);
            result.Json.Should().BeNull();
        }

        [Test]
        public async Task When_Querying_An_EndPoint_Thats_Returns_Badly_Formated_Json_Value()
        {
            SetupBadlyFormedJsonResponseFromInnerApi();

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
            _httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("") };
            _httpClientHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(x => true),
                    ItExpr.IsAny<CancellationToken>()).ReturnsAsync(_httpResponseMessage);
        }

        private void SetupBadlyFormedJsonResponseFromInnerApi()
        {
            _httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"Rubbish? json \"XXXX\"}", Encoding.UTF8, "application/json") };
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
    }
}

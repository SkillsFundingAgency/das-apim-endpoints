using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.SharedOuterApi.UnitTests.Infrastructure.RestApiClientTests
{
    [TestFixture]
    public class WhenPostingToEndpoints
    {
        private HttpClient _httpClient;
        private Mock<HttpClientHandler> _httpClientHandlerMock;
        public HttpResponseMessage _httpResponseMessage;
        private RestApiClient _sut;
        private Fixture _fixture;
        private TestResponse _testResponse;
        private TestRequest _testRequest;
        private string _baseUrl = "http://www.someurl.com";
        private string _simpleResponse;

        [SetUp]
        public void Arrange()
        {
            _fixture = new Fixture();
            _simpleResponse = _fixture.Create<string>();
            _testRequest = _fixture.Create<TestRequest>();
            _testResponse = _fixture.Create<TestResponse>();

            _httpClientHandlerMock = new Mock<HttpClientHandler>();

            _httpClient = new HttpClient(_httpClientHandlerMock.Object);
            _httpClient.BaseAddress = new Uri(_baseUrl);

            _sut = new RestApiClient(_httpClient);
        }

        [TestCase("/test/123")]
        public async Task And_Post_Is_Called_With_RequestType_And_Expected_ResponseType_Then_It_Should_Return_The_ResponseType(string path)
        {
            SetupJsonOkResponseForPath(path);

            var result = await _sut.Post<TestRequest, TestResponse>(path, _testRequest);

            result.Should().BeEquivalentTo(_testResponse);
        }

        [TestCase("/test/123")]
        public async Task
            And_Post_Is_Called_With_RequestType_And_Incorrectly_Casing_Of_Properties_For_Expected_ResponseType_Then_It_Should_Still_Return_The_ResponseType(
                string path)
        {
            SetupJsonOkResponseForPathButWithIncorrectCasingOfPropertyNames(path);

            var result = await _sut.Post<TestRequest, TestResponse>(path, _testRequest);

            result.Should().NotBeNull();
            result.StringValue.Should().Be("Expected");
            result.NumberValue.Should().Be(21);
            result.DecimalValue.Should().Be(1.1m);
        }

        [TestCase("/test/123")]
        public async Task And_Post_Is_Called_With_No_RequestType_And_No_Expected_ResponseType_Then_It_Should_Return_String_Content(string path)
        {
            SetupStringOkResponseForPath(path);

            var result = await _sut.Post(path);

            result.Should().Be(_simpleResponse);
        }

        [TestCase("/test/123")]
        public async Task And_Post_Is_Called_With_Only_CancellationToken_And_No_Expected_ResponseType_Then_It_Should_Return_String_Content(string path)
        {
            SetupStringOkResponseForPath(path);

            var result = await _sut.Post(path, CancellationToken.None);

            result.Should().Be(_simpleResponse);
        }

        [TestCase("/test/123")]
        public async Task And_Post_Is_Called_With_RequestType_And_No_Expected_ResponseType_Then_It_Should_Return_String_Content(string path)
        {
            SetupStringOkResponseForPath(path);

            var result = await _sut.Post<TestRequest>(path, _testRequest);

            result.Should().Be(_simpleResponse);
        }

        private void SetupJsonOkResponseForPath(string path)
        {
            _httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(JsonSerializer.Serialize(_testResponse), Encoding.UTF8, "application/json") };
            SetupMockResponse(path);
        }

        private void SetupJsonOkResponseForPathButWithIncorrectCasingOfPropertyNames(string path)
        {
            _httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(SetupIncorrectCasingForResponseType(), Encoding.UTF8, "application/json") };
            SetupMockResponse(path);
        }

        private void SetupStringOkResponseForPath(string path)
        {
            _httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(_simpleResponse) };
            SetupMockResponse(path);
        }

        private void SetupMockResponse(string path)
        {
            var uri = new Uri(_baseUrl + path, UriKind.RelativeOrAbsolute);
            _httpClientHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(x => x.RequestUri == uri),
                    ItExpr.IsAny<CancellationToken>()).ReturnsAsync(_httpResponseMessage);
        }

        private string SetupIncorrectCasingForResponseType()
        {
            return "{\"stringVALUE\":\"Expected\",\"NUmBerValuE\":21,\"decimalvalue\":1.1}";
        }

        public class TestResponse
        {
            public string StringValue { get; set; }
            public long NumberValue { get; set; }
            public Decimal DecimalValue { get; set; }
        }

        public class TestRequest
        {
            public long Id { get; set; }
            public string InputData { get; set; }
        }
    }
}
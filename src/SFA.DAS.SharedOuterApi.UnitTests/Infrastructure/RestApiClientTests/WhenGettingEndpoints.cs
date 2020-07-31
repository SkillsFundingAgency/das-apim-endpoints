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
    public class WhenGettingEndpoints
    {
        private HttpClient _httpClient;
        private Mock<HttpClientHandler> _httpClientHandlerMock;
        public HttpResponseMessage _httpResponseMessage;
        private RestApiClient _sut;
        private Fixture _fixture;
        private TestResponse _testResponse;
        private string _baseUrl = "http://www.someurl.com";
        private string _simpleResponse;

        [SetUp]
        public void Arrange()
        {
            _fixture = new Fixture();
            _simpleResponse = _fixture.Create<string>();
            _testResponse = _fixture.Create<TestResponse>();

            _httpClientHandlerMock = new Mock<HttpClientHandler>();

            _httpClient = new HttpClient(_httpClientHandlerMock.Object);
            _httpClient.BaseAddress = new Uri(_baseUrl);

            _sut = new RestApiClient(_httpClient);
        }

        [TestCase("/test/123")]
        [TestCase("/test/123?abc=1")]
        [TestCase("/test/123?abc=1&some=XXX")]
        public async Task And_Get_Is_Called_With_No_Return_Type_And_No_QueryStringObject_Then_It_Should_Return_String_Content(string path)
        {

            SetupStringOkResponseForPath(path);

            var result = await _sut.Get(path);

            result.Should().Be(_simpleResponse);
        }

        [TestCase("/test/123")]
        [TestCase("/test/123?abc=1")]
        [TestCase("/test/123?abc=1&some=XXX")]
        public async Task And_Get_Is_Called_With_No_Return_Type_And_A_CancellationToken_Then_It_Should_Return_String_Content(string path)
        {

            SetupStringOkResponseForPath(path);

            var result = await _sut.Get(path, CancellationToken.None);

            result.Should().Be(_simpleResponse);
        }


        [TestCase("/test/123")]
        public async Task And_Get_Is_Called_With_No_Return_Type_And_With_A_QueryStringObject_Then_It_Should_Return_String_Content(string path)
        {
            var queryObject = new { abc = 1 };

            SetupStringOkResponseForPath(path + "?abc=1");

            var result = await _sut.Get(path, queryObject);

            result.Should().Be(_simpleResponse);
        }


        [TestCase("/test/123")]
        public async Task And_Get_Is_Called_With_A_Return_Type_And_No_QueryObject_Then_It_Should_Return_Json_Content(string path)
        {
            SetupJsonOkResponseForPath(path);

            var result = await _sut.Get<TestResponse>(path);

            result.Should().BeEquivalentTo(_testResponse);
        }

        [TestCase("/test/123")]
        public async Task And_Get_Is_Called_With_A_Return_Type_And_With_A_CancellationToken_Then_It_Should_Return_Json_Content(string path)
        {
            SetupJsonOkResponseForPath(path);

            var result = await _sut.Get<TestResponse>(path, CancellationToken.None);

            result.Should().BeEquivalentTo(_testResponse);
        }

        [TestCase("/test/123")]
        public async Task And_Get_Is_Called_With_Return_Type_And_With_A_QueryStringObject_Then_It_Should_Return_Json_Content(string path)
        {
            var queryObject = new { abc = 1 };

            SetupJsonOkResponseForPath(path + "?abc=1");

            var result = await _sut.Get<TestResponse>(path, queryObject);

            result.Should().BeEquivalentTo(_testResponse);
        }

        [TestCase("/test/123", HttpStatusCode.NoContent)]
        [TestCase("/test/123", HttpStatusCode.Found)]
        [TestCase("/test/123", HttpStatusCode.NotFound)]
        public async Task And_GetHttpStatusCode_Is_Called_With_A_QueryStringObject_Then_It_Should_Return_The_StatusCode(string path, HttpStatusCode statusCode)
        {
            var queryObject = new { abc = 1 };

            SetupHttpStatusCodeResponseForPath(path + "?abc=1", statusCode);

            var result = await _sut.GetHttpStatusCode(path, queryObject);

            result.Should().Be(statusCode);
        }

        [TestCase("/test/123", HttpStatusCode.NoContent)]
        [TestCase("/test/123", HttpStatusCode.Found)]
        [TestCase("/test/123", HttpStatusCode.NotFound)]
        public async Task And_GetHttpStatusCode_Is_Called_With_A_CancellationToken_Then_It_Should_Return_The_StatusCode(string path, HttpStatusCode statusCode)
        {
            SetupHttpStatusCodeResponseForPath(path, statusCode);

            var result = await _sut.GetHttpStatusCode(path, CancellationToken.None);

            result.Should().Be(statusCode);
        }

        private void SetupJsonOkResponseForPath(string path)
        {
            _httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(JsonSerializer.Serialize(_testResponse), Encoding.UTF8, "application/json") };
            SetupMockResponse(path);
        }

        private void SetupStringOkResponseForPath(string path)
        {
            _httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(_simpleResponse) };
            SetupMockResponse(path);
        }

        private void SetupHttpStatusCodeResponseForPath(string path, HttpStatusCode statusCode)
        {
            _httpResponseMessage = new HttpResponseMessage(statusCode);
            SetupMockResponse(path);
        }

        private void SetupMockResponse(string path)
        {
            var uri = new Uri(_baseUrl + path, UriKind.RelativeOrAbsolute);
            _httpClientHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(x => x.RequestUri == uri),
                    ItExpr.IsAny<CancellationToken>()).ReturnsAsync(_httpResponseMessage);
        }

        public class TestResponse
        {
            public string StringValue { get; set; }
            public long NumberValue { get; set; }
            public Decimal DecimalValue { get; set; }
            public DateTime DateTimeValue { get; set; }
        }
    }
}

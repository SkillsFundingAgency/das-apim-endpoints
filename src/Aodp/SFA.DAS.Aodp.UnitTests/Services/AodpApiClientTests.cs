using System.Net;
using System.Net.Http.Headers;
using Moq;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi;
using SFA.DAS.Aodp.Services;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Aodp.UnitTests.Services;

[TestFixture]
public class AodpApiClientTests
{
    [Test]
    public async Task PostWithResponseCodeAsMultipart_WhenRequestIsProvided_SendsAuthenticatedMultipartContent()
    {
        // Arrange
        var messageHandler = new RecordingMessageHandler();
        var httpClient = new HttpClient(messageHandler);
        var httpClientFactory = new Mock<IHttpClientFactory>();
        var credentialHelper = new Mock<IAzureClientCredentialHelper>();
        var internalApiClient = new Mock<IInternalApiClient<AodpApiConfiguration>>();
        var configuration = new AodpApiConfiguration
        {
            Url = "https://inner-api.test/",
            Identifier = "inner-api-identifier"
        };
        var request = new TestMultipartRequest(
            "api/rollover/submitrolloverextension",
            [
                new KeyValuePair<string, string>("Items[0].Qan", "12345678"),
                new KeyValuePair<string, string>("Items[0].FundingStreamName", "FS1")
            ]);

        httpClientFactory.Setup(factory => factory.CreateClient(string.Empty)).Returns(httpClient);
        credentialHelper
            .Setup(helper => helper.GetAccessTokenAsync(configuration.Identifier))
            .ReturnsAsync("access-token");
        var sut = new AodpApiClient(
            internalApiClient.Object,
            httpClientFactory.Object,
            configuration,
            credentialHelper.Object);

        // Act
        var result = await sut.PostWithResponseCodeAsMultipart<TestResponse>(request);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(result.Body.ResultMessage, Is.EqualTo("Applied"));
            Assert.That(messageHandler.RequestUri, Is.EqualTo(
                new Uri("https://inner-api.test/api/rollover/submitrolloverextension")));
            Assert.That(messageHandler.ContentType, Does.StartWith("multipart/form-data"));
            Assert.That(messageHandler.Body, Does.Contain("Items[0].Qan"));
            Assert.That(messageHandler.Body, Does.Contain("12345678"));
            Assert.That(messageHandler.Authorization, Is.EqualTo(
                new AuthenticationHeaderValue("Bearer", "access-token")));
        });
    }

    [Test]
    public async Task PostWithResponseCodeAsJsonFile_WhenRequestIsProvided_SendsOneJsonFile()
    {
        // Arrange
        var messageHandler = new RecordingMessageHandler();
        var httpClientFactory = new Mock<IHttpClientFactory>();
        httpClientFactory.Setup(factory => factory.CreateClient(string.Empty))
            .Returns(new HttpClient(messageHandler));
        var sut = new AodpApiClient(
            Mock.Of<IInternalApiClient<AodpApiConfiguration>>(),
            httpClientFactory.Object,
            new AodpApiConfiguration { Url = "https://inner-api.test/" },
            Mock.Of<IAzureClientCredentialHelper>());
        var request = new TestJsonFileRequest(
            "api/rollover/submitrolloverextension",
            new { Items = new[] { new { Qan = "12345678" } } });

        // Act
        await sut.PostWithResponseCodeAsJsonFile<TestResponse>(request);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(messageHandler.ContentType, Does.StartWith("multipart/form-data"));
            Assert.That(messageHandler.ContentType, Does.Not.Contain("\""));
            Assert.That(messageHandler.Body, Does.Contain("name=payload; filename=payload.json"));
            Assert.That(messageHandler.Body, Does.Contain("Content-Type: application/json"));
            Assert.That(messageHandler.Body, Does.Contain("\"Qan\":\"12345678\""));
        });
    }

    private sealed record TestMultipartRequest(
        string PostUrl,
        IEnumerable<KeyValuePair<string, string>> FormData)
        : IPostMultipartFormDataApiRequest;

    private sealed record TestJsonFileRequest(string PostUrl, object Data)
        : IPostMultipartJsonFileApiRequest;

    private sealed class TestResponse
    {
        public string ResultMessage { get; set; } = string.Empty;
    }

    private sealed class RecordingMessageHandler : HttpMessageHandler
    {
        public Uri? RequestUri { get; private set; }
        public string? ContentType { get; private set; }
        public string? Body { get; private set; }
        public AuthenticationHeaderValue? Authorization { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            RequestUri = request.RequestUri;
            ContentType = request.Content?.Headers.ContentType?.ToString();
            Body = request.Content == null
                ? null
                : await request.Content.ReadAsStringAsync(cancellationToken);
            Authorization = request.Headers.Authorization;

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"resultMessage\":\"Applied\"}")
            };
        }
    }
}

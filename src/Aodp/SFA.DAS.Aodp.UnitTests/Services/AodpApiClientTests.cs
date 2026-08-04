using System.Net;
using System.Net.Http.Headers;
using Moq;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi;
using SFA.DAS.Aodp.Services;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;

namespace SFA.DAS.Aodp.UnitTests.Services;

[TestFixture]
public class AodpApiClientTests
{
    [Test]
    public async Task Get_WhenCalled_DelegatesToInternalApiClient()
    {
        // Arrange
        var request = Mock.Of<IGetApiRequest>();
        var expected = new TestResponse { ResultMessage = "Applied" };
        var internalApiClient = new Mock<IInternalApiClient<AodpApiConfiguration>>();
        internalApiClient.Setup(client => client.Get<TestResponse>(request)).ReturnsAsync(expected);
        var sut = CreateSut(internalApiClient);

        // Act
        var result = await sut.Get<TestResponse>(request);

        // Assert
        Assert.That(result, Is.SameAs(expected));
        internalApiClient.Verify(client => client.Get<TestResponse>(request), Times.Once);
    }

    [Test]
    public async Task GetResponseCode_WhenCalled_DelegatesToInternalApiClient()
    {
        // Arrange
        var request = Mock.Of<IGetApiRequest>();
        var internalApiClient = new Mock<IInternalApiClient<AodpApiConfiguration>>();
        internalApiClient.Setup(client => client.GetResponseCode(request)).ReturnsAsync(HttpStatusCode.Accepted);
        var sut = CreateSut(internalApiClient);

        // Act
        var result = await sut.GetResponseCode(request);

        // Assert
        Assert.That(result, Is.EqualTo(HttpStatusCode.Accepted));
        internalApiClient.Verify(client => client.GetResponseCode(request), Times.Once);
    }

    [Test]
    public async Task GetWithResponseCode_WhenCalled_DelegatesToInternalApiClient()
    {
        // Arrange
        var request = Mock.Of<IGetApiRequest>();
        var expected = new ApiResponse<TestResponse>(new TestResponse(), HttpStatusCode.OK, string.Empty);
        var internalApiClient = new Mock<IInternalApiClient<AodpApiConfiguration>>();
        internalApiClient.Setup(client => client.GetWithResponseCode<TestResponse>(request)).ReturnsAsync(expected);
        var sut = CreateSut(internalApiClient);

        // Act
        var result = await sut.GetWithResponseCode<TestResponse>(request);

        // Assert
        Assert.That(result, Is.SameAs(expected));
        internalApiClient.Verify(client => client.GetWithResponseCode<TestResponse>(request), Times.Once);
    }

    [Test]
    public async Task Delete_WhenCalled_DelegatesToInternalApiClient()
    {
        // Arrange
        var request = Mock.Of<IDeleteApiRequest>();
        var internalApiClient = new Mock<IInternalApiClient<AodpApiConfiguration>>();
        internalApiClient.Setup(client => client.Delete(request)).Returns(Task.CompletedTask);
        var sut = CreateSut(internalApiClient);

        // Act
        await sut.Delete(request);

        // Assert
        internalApiClient.Verify(client => client.Delete(request), Times.Once);
    }

    [Test]
    public async Task DeleteWithResponseCode_WhenCalled_DelegatesRequestAndIncludeResponse()
    {
        // Arrange
        var request = Mock.Of<IDeleteApiRequest>();
        var expected = new ApiResponse<TestResponse>(new TestResponse(), HttpStatusCode.OK, string.Empty);
        var internalApiClient = new Mock<IInternalApiClient<AodpApiConfiguration>>();
        internalApiClient
            .Setup(client => client.DeleteWithResponseCode<TestResponse>(request, true))
            .ReturnsAsync(expected);
        var sut = CreateSut(internalApiClient);

        // Act
        var result = await sut.DeleteWithResponseCode<TestResponse>(request, true);

        // Assert
        Assert.That(result, Is.SameAs(expected));
        internalApiClient.Verify(
            client => client.DeleteWithResponseCode<TestResponse>(request, true),
            Times.Once);
    }

    [Test]
    public async Task Put_WhenCalled_DelegatesToInternalApiClient()
    {
        // Arrange
        var request = Mock.Of<IPutApiRequest>();
        var internalApiClient = new Mock<IInternalApiClient<AodpApiConfiguration>>();
        internalApiClient.Setup(client => client.Put(request)).Returns(Task.CompletedTask);
        var sut = CreateSut(internalApiClient);

        // Act
        await sut.Put(request);

        // Assert
        internalApiClient.Verify(client => client.Put(request), Times.Once);
    }

    [Test]
    public async Task PostWithResponseCode_WhenCalled_DelegatesToInternalApiClient()
    {
        // Arrange
        var request = Mock.Of<IPostApiRequest>();
        var expected = new ApiResponse<TestResponse>(new TestResponse(), HttpStatusCode.OK, string.Empty);
        var internalApiClient = new Mock<IInternalApiClient<AodpApiConfiguration>>();
        internalApiClient
            .Setup(client => client.PostWithResponseCode<TestResponse>(request, It.IsAny<bool>()))
            .ReturnsAsync(expected);
        var sut = CreateSut(internalApiClient);

        // Act
        var result = await sut.PostWithResponseCode<TestResponse>(request, false);

        // Assert
        Assert.That(result, Is.SameAs(expected));
        internalApiClient.Verify(
            client => client.PostWithResponseCode<TestResponse>(request, It.IsAny<bool>()),
            Times.Once);
    }

    [Test]
    public async Task PatchWithResponseCode_WhenCalled_DelegatesToInternalApiClient()
    {
        // Arrange
        var request = Mock.Of<IPatchApiRequest<TestData>>();
        var expected = new ApiResponse<string>("Applied", HttpStatusCode.OK, string.Empty);
        var internalApiClient = new Mock<IInternalApiClient<AodpApiConfiguration>>();
        internalApiClient
            .Setup(client => client.PatchWithResponseCode<TestData>(request))
            .ReturnsAsync(expected);
        var sut = CreateSut(internalApiClient);

        // Act
        var result = await sut.PatchWithResponseCode<TestData>(request);

        // Assert
        Assert.That(result, Is.SameAs(expected));
        internalApiClient.Verify(client => client.PatchWithResponseCode<TestData>(request), Times.Once);
    }

    [Test]
    public async Task PutWithResponseCode_WhenCalled_DelegatesToInternalApiClient()
    {
        // Arrange
        var request = Mock.Of<IPutApiRequest>();
        var expected = new ApiResponse<TestResponse>(new TestResponse(), HttpStatusCode.OK, string.Empty);
        var internalApiClient = new Mock<IInternalApiClient<AodpApiConfiguration>>();
        internalApiClient
            .Setup(client => client.PutWithResponseCode<TestResponse>(request))
            .ReturnsAsync(expected);
        var sut = CreateSut(internalApiClient);

        // Act
        var result = await sut.PutWithResponseCode<TestResponse>(request);

        // Assert
        Assert.That(result, Is.SameAs(expected));
        internalApiClient.Verify(client => client.PutWithResponseCode<TestResponse>(request), Times.Once);
    }

    [Test]
    public async Task PostWithResponseCodeAsMultipart_WhenRequestIsProvided_SendsAuthenticatedMultipartContent()
    {
        // Arrange
        var messageHandler = new RecordingMessageHandler();
        var credentialHelper = new Mock<IAzureClientCredentialHelper>();
        var configuration = new AodpApiConfiguration
        {
            Url = "https://inner-api.test/",
            Identifier = "inner-api-identifier"
        };
        credentialHelper
            .Setup(helper => helper.GetAccessTokenAsync(configuration.Identifier))
            .ReturnsAsync("access-token");
        var sut = CreateSut(messageHandler, configuration, credentialHelper.Object);
        var request = new TestMultipartRequest(
            "api/rollover/submitrolloverextension",
            [
                new KeyValuePair<string, string>("Items[0].Qan", "12345678"),
                new KeyValuePair<string, string>("Items[0].FundingStreamName", "FS1")
            ]);

        // Act
        var result = await sut.PostWithResponseCodeAsMultipart<TestResponse>(request);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(result.Body.ResultMessage, Is.EqualTo("Applied"));
            Assert.That(result.ErrorContent, Does.Contain("resultMessage"));
            Assert.That(messageHandler.RequestUri, Is.EqualTo(
                new Uri("https://inner-api.test/api/rollover/submitrolloverextension")));
            Assert.That(messageHandler.ContentType, Does.StartWith("multipart/form-data"));
            Assert.That(messageHandler.Body, Does.Contain("Items[0].Qan"));
            Assert.That(messageHandler.Body, Does.Contain("12345678"));
            Assert.That(messageHandler.Authorization, Is.EqualTo(
                new AuthenticationHeaderValue("Bearer", "access-token")));
            Assert.That(messageHandler.XVersion, Is.EqualTo("1.0"));
        });
        credentialHelper.Verify(
            helper => helper.GetAccessTokenAsync(configuration.Identifier),
            Times.Once);
    }

    [Test]
    public async Task PostWithResponseCodeAsMultipart_WhenAuthenticationIsNotConfiguredAndBodyIsEmpty_ReturnsStatusWithoutBody()
    {
        // Arrange
        var messageHandler = new RecordingMessageHandler(HttpStatusCode.BadRequest, "   ");
        var cancellationTokenSource = new CancellationTokenSource();
        var sut = CreateSut(
            messageHandler,
            new AodpApiConfiguration { Url = "https://inner-api.test/" });
        var request = new TestMultipartRequest("api/rollover/validate", []);

        // Act
        var result = await sut.PostWithResponseCodeAsMultipart<TestResponse>(
            request,
            cancellationTokenSource.Token);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(result.Body, Is.Null);
            Assert.That(result.ErrorContent, Is.EqualTo("   "));
            Assert.That(messageHandler.Authorization, Is.Null);
            Assert.That(messageHandler.CancellationToken.CanBeCanceled, Is.True);
        });
    }

    [Test]
    public async Task PostWithResponseCodeAsJsonFile_WhenRequestIsProvided_SendsOneJsonFileWithoutAuthentication()
    {
        // Arrange
        var messageHandler = new RecordingMessageHandler();
        var sut = CreateSut(
            messageHandler,
            new AodpApiConfiguration { Url = "https://inner-api.test/" });
        var request = new TestJsonFileRequest(
            "api/rollover/submitrolloverextension",
            new { Items = new[] { new { Qan = "12345678" } } });

        // Act
        var result = await sut.PostWithResponseCodeAsJsonFile<TestResponse>(request);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Body.ResultMessage, Is.EqualTo("Applied"));
            Assert.That(messageHandler.ContentType, Does.StartWith("multipart/form-data"));
            Assert.That(messageHandler.ContentType, Does.Not.Contain("\""));
            Assert.That(messageHandler.Body, Does.Contain("name=payload; filename=payload.json"));
            Assert.That(messageHandler.Body, Does.Contain("Content-Type: application/json"));
            Assert.That(messageHandler.Body, Does.Contain("\"Qan\":\"12345678\""));
            Assert.That(messageHandler.Authorization, Is.Null);
            Assert.That(messageHandler.XVersion, Is.EqualTo("1.0"));
        });
    }

    [Test]
    public async Task PostWithResponseCodeAsJsonFile_WhenAuthenticatedResponseBodyIsEmpty_ReturnsStatusWithoutBody()
    {
        // Arrange
        var messageHandler = new RecordingMessageHandler(HttpStatusCode.ServiceUnavailable, string.Empty);
        var credentialHelper = new Mock<IAzureClientCredentialHelper>();
        var configuration = new AodpApiConfiguration
        {
            Url = "https://inner-api.test/",
            Identifier = "inner-api-identifier"
        };
        credentialHelper
            .Setup(helper => helper.GetAccessTokenAsync(configuration.Identifier))
            .ReturnsAsync("access-token");
        var cancellationTokenSource = new CancellationTokenSource();
        var sut = CreateSut(messageHandler, configuration, credentialHelper.Object);
        var request = new TestJsonFileRequest("api/rollover/validate", new { Items = Array.Empty<object>() });

        // Act
        var result = await sut.PostWithResponseCodeAsJsonFile<TestResponse>(
            request,
            cancellationTokenSource.Token);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.ServiceUnavailable));
            Assert.That(result.Body, Is.Null);
            Assert.That(result.ErrorContent, Is.Empty);
            Assert.That(messageHandler.Authorization, Is.EqualTo(
                new AuthenticationHeaderValue("Bearer", "access-token")));
            Assert.That(messageHandler.CancellationToken.CanBeCanceled, Is.True);
        });
    }

    private static AodpApiClient CreateSut(
        Mock<IInternalApiClient<AodpApiConfiguration>> internalApiClient)
    {
        return new AodpApiClient(
            internalApiClient.Object,
            Mock.Of<IHttpClientFactory>(),
            new AodpApiConfiguration(),
            Mock.Of<IAzureClientCredentialHelper>());
    }

    private static AodpApiClient CreateSut(
        RecordingMessageHandler messageHandler,
        AodpApiConfiguration configuration,
        IAzureClientCredentialHelper? credentialHelper = null)
    {
        var httpClientFactory = new Mock<IHttpClientFactory>();
        httpClientFactory.Setup(factory => factory.CreateClient(string.Empty))
            .Returns(new HttpClient(messageHandler));

        return new AodpApiClient(
            Mock.Of<IInternalApiClient<AodpApiConfiguration>>(),
            httpClientFactory.Object,
            configuration,
            credentialHelper ?? Mock.Of<IAzureClientCredentialHelper>());
    }

    private sealed record TestMultipartRequest(
        string PostUrl,
        IEnumerable<KeyValuePair<string, string>> FormData)
        : IPostMultipartFormDataApiRequest;

    private sealed record TestJsonFileRequest(string PostUrl, object Data)
        : IPostMultipartJsonFileApiRequest;

    public sealed class TestData;

    private sealed class TestResponse
    {
        public string ResultMessage { get; set; } = string.Empty;
    }

    private sealed class RecordingMessageHandler(
        HttpStatusCode statusCode = HttpStatusCode.OK,
        string responseContent = "{\"resultMessage\":\"Applied\"}") : HttpMessageHandler
    {
        public Uri? RequestUri { get; private set; }
        public string? ContentType { get; private set; }
        public string? Body { get; private set; }
        public AuthenticationHeaderValue? Authorization { get; private set; }
        public string? XVersion { get; private set; }
        public CancellationToken CancellationToken { get; private set; }

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
            XVersion = request.Headers.GetValues("X-Version").Single();
            CancellationToken = cancellationToken;

            return new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(responseContent)
            };
        }
    }
}

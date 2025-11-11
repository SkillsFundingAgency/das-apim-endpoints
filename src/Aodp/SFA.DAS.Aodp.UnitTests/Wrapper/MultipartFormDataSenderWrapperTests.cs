using Microsoft.AspNetCore.Http;
using Moq;
using Moq.Protected;
using SFA.DAS.Aodp.Application.Commands.Import;
using SFA.DAS.Aodp.InnerApi.AodpApi.Import;
using SFA.DAS.Aodp.Wrapper;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace SFA.DAS.Aodp.UnitTests.Wrapper;

[TestFixture]
public class MultipartFormDataSenderWrapperTests
{
    private const string BaseUrl = "https://api.test";
    private AodpApiConfiguration _config = null!;

    [SetUp]
    public void SetUp()
    {
        _config = new AodpApiConfiguration { Url = BaseUrl };
    }


    [Test]
    public void Constructor_WithValidDependencies_SetsPrivateFields()
    {
        // Arrange
        var httpFactoryMock = new Mock<IHttpClientFactory>();
        var config = new AodpApiConfiguration { Url = "https://api.test" };

        // Act
        var sut = new MultipartFormDataSenderWrapper(httpFactoryMock.Object, config);

        // Assert
        Assert.That(sut, Is.Not.Null);

        var type = typeof(MultipartFormDataSenderWrapper);
        var httpFactoryField = type.GetField("_httpClientFactory", BindingFlags.Instance | BindingFlags.NonPublic);
        var configField = type.GetField("_aodpApiConfiguration", BindingFlags.Instance | BindingFlags.NonPublic);

        Assert.That(httpFactoryField, Is.Not.Null, "Could not find _httpClientFactory field via reflection");
        Assert.That(configField, Is.Not.Null, "Could not find _aodpApiConfiguration field via reflection");

        var httpFactoryValue = httpFactoryField!.GetValue(sut);
        var configValue = configField!.GetValue(sut);

        Assert.That(ReferenceEquals(httpFactoryValue, httpFactoryMock.Object), Is.True);
        Assert.That(ReferenceEquals(configValue, config), Is.True);
    }

    [Test]
    public void Constructor_AllowsNullDependencies_SetsPrivateFieldsToNull()
    {
        // Arrange / Act
        var sut = new MultipartFormDataSenderWrapper(null!, null!);

        // Assert
        Assert.That(sut, Is.Not.Null);

        var type = typeof(MultipartFormDataSenderWrapper);
        var httpFactoryField = type.GetField("_httpClientFactory", BindingFlags.Instance | BindingFlags.NonPublic);
        var configField = type.GetField("_aodpApiConfiguration", BindingFlags.Instance | BindingFlags.NonPublic);

        var httpFactoryValue = httpFactoryField!.GetValue(sut);
        var configValue = configField!.GetValue(sut);

        Assert.That(httpFactoryValue, Is.Null);
        Assert.That(configValue, Is.Null);
    }

    [Test]
    public async Task PostWithMultipartFormData_WhenStatusCode200_AndIncludeResponseTrue_ShouldReturnsDeserializedResponse()
    {
        // Arrange
        var expected = new ImportDefundingListResponse { Message = "ok", ImportedCount = 10 };
        var expectedJson = JsonSerializer.Serialize(expected);
        var handlerMock = CreateHandlerMock(expectedJson);

        var httpClient = CreateHttpClient(handlerMock);

        var httpFactoryMock = new Mock<IHttpClientFactory>();
        httpFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);
        var wrapper = new MultipartFormDataSenderWrapper(httpFactoryMock.Object, _config);
        var request = GetImportDefundingListApiRequest();

        // Act
        var result = await wrapper.PostWithMultipartFormData<IFormFile, ImportDefundingListResponse>(request, includeResponse: true);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(result.ErrorContent, Is.EqualTo(string.Empty));
            Assert.That(result.Body, Is.Not.Null);
            Assert.That(result.Body!.Message, Is.EqualTo(expected.Message));
            Assert.That(result.Body.ImportedCount, Is.EqualTo(expected.ImportedCount));

            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );
        });
    }

    [Test]
    public async Task PostWithMultipartFormData_WhenIncludeResponseFalse_ShouldNotDeserialize()
    {
        // Arrange
        var expected = new ImportDefundingListResponse { Message = "ok", ImportedCount = 5 };
        var expectedJson = JsonSerializer.Serialize(expected);

        var handlerMock = CreateHandlerMock(expectedJson);
        var httpClient = CreateHttpClient(handlerMock);

        var httpFactoryMock = new Mock<IHttpClientFactory>();
        httpFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var wrapper = new MultipartFormDataSenderWrapper(httpFactoryMock.Object, _config);
        var request = GetImportDefundingListApiRequest();

        // Act
        var result = await wrapper.PostWithMultipartFormData<IFormFile, ImportDefundingListResponse>(request, includeResponse: false);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(result.ErrorContent, Is.EqualTo(string.Empty));
            Assert.That(result.Body, Is.EqualTo(default(ImportDefundingListResponse)));

            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );
        });
    }

    [Test]
    public async Task PostWithMultipartFormData_WhenNon200Status_ShouldReturnsErrorContent()
    {
        // Arrange
        var errorBody = "{ \"error\": \"bad request\" }";

        var handlerMock = CreateHandlerMock(errorBody, HttpStatusCode.BadRequest);
        var httpClient = CreateHttpClient(handlerMock);

        var httpFactoryMock = new Mock<IHttpClientFactory>();
        httpFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var wrapper = new MultipartFormDataSenderWrapper(httpFactoryMock.Object, _config);
        var request = GetImportDefundingListApiRequest();

        // Act
        var result = await wrapper.PostWithMultipartFormData<IFormFile, ImportDefundingListResponse>(request, includeResponse: true);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(result.Body, Is.Null);
            Assert.That(result.ErrorContent, Is.EqualTo(errorBody));
        });

        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    private static HttpClient CreateHttpClient(Mock<HttpMessageHandler> handlerMock)
    {
        var client = new HttpClient(handlerMock.Object, disposeHandler: false);
        return client;
    }

    private Mock<HttpMessageHandler> CreateHandlerMock(string expectedJson, HttpStatusCode code = HttpStatusCode.OK)
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync((HttpRequestMessage request, CancellationToken ct) =>
                {
                    var response = new HttpResponseMessage(code)
                    {
                        Content = new StringContent(expectedJson, Encoding.UTF8, "application/json")
                    };
                    return response;
                })
                .Verifiable();

        return handlerMock;
    }

    private static Mock<IFormFile> CreateFormFile(string fileName, string contentType, byte[] contentBytes)
    {
        var ms = new MemoryStream(contentBytes);
        var formFileMock = new Mock<IFormFile>();
        formFileMock.Setup(f => f.FileName).Returns(fileName);
        formFileMock.Setup(f => f.ContentType).Returns(contentType);
        formFileMock.Setup(f => f.OpenReadStream()).Returns(ms);
        return formFileMock;
    }

    private static ImportDefundingListApiRequest GetImportDefundingListApiRequest()
    {
        var fileBytes = Encoding.UTF8.GetBytes("file-content");
        var formFileMock = CreateFormFile("bad.txt", "text/plain", fileBytes);

        return new ImportDefundingListApiRequest
        {
            Data = formFileMock.Object
        };
    }
}
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Configuration;
using SFA.DAS.FindApprenticeshipJobs.Services;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.Services;
[TestFixture]
public class WhenCallingGetCivilServiceJobsApiClient
{
    [Test, AutoData]
    public async Task Then_The_Endpoint_Is_Called(
        string apiResponse,
        CivilServiceJobsConfiguration config)
    {
        //Arrange
        config.Url = "https://test.local";
        config.ApiKey = "some-secret-key";
        var response = new HttpResponseMessage
        {
            Content = new StringContent($"{apiResponse}"),
            StatusCode = HttpStatusCode.Accepted
        };
        var getTestRequest = new GetTestRequest();
        var expectedUrl = $"{config.Url}/{getTestRequest.GetUrl}";
        var httpMessageHandler = new Mock<HttpMessageHandler>();
        httpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(c =>
                    c.Method.Equals(HttpMethod.Get)
                    && c.RequestUri!.AbsoluteUri.Equals(expectedUrl)
                    && c.Headers.Contains("X-Api-Key")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync((HttpRequestMessage _, CancellationToken _) => response);

        var client = new HttpClient(httpMessageHandler.Object);
        var clientFactory = new Mock<IHttpClientFactory>();
        var logger = new Mock<ILogger<CivilServiceJobsApiClient>>();



        clientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
        var actualClient = new CivilServiceJobsApiClient(logger.Object, clientFactory.Object, config);

        //Act
        var actual = await actualClient.GetWithResponseCode(getTestRequest);

        //Assert
        httpMessageHandler.Protected()
            .Verify<Task<HttpResponseMessage>>(
                "SendAsync", Times.Once(),
                ItExpr.Is<HttpRequestMessage>(c =>
                    c.Method.Equals(HttpMethod.Get)
                    && c.RequestUri.AbsoluteUri.Equals(expectedUrl)),
                ItExpr.IsAny<CancellationToken>()
            );
        actual.StatusCode.Should().Be(HttpStatusCode.Accepted);
        actual.Body.Should().Be(apiResponse);
    }
    private class GetTestRequest : IGetApiRequest
    {
        public string GetUrl => "test-url";
    }
}

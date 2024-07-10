using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Configuration;
using SFA.DAS.FindApprenticeshipJobs.Services;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.Services;

public class WhenCallingGetOnNhsApiClient
{
    [Test, AutoData]
    public async Task Then_The_Endpoint_Is_Called(
        string authToken,
        int id,
        NhsJobsConfiguration config)
    {
        //Arrange
        
        config.Url = "https://test.local";
        var response = new HttpResponseMessage
        {
            Content = new StringContent("\"test\""),
            StatusCode = HttpStatusCode.Accepted
        };
        var getTestRequest = new GetTestRequest(id);
        var expectedUrl = $"{config.Url}/{getTestRequest.GetUrl}";
        var httpMessageHandler = new Mock<HttpMessageHandler>();
        httpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(c =>
                    c.Method.Equals(HttpMethod.Get)
                    && c.RequestUri.AbsoluteUri.Equals(expectedUrl)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync((HttpRequestMessage request, CancellationToken token) => response);
        
        var client = new HttpClient(httpMessageHandler.Object);
        var clientFactory = new Mock<IHttpClientFactory>();
        clientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
        var actualClient = new NhsJobsApiClient(clientFactory.Object, config);

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
        actual.Body.Should().Be("\"test\"");
    }
    private class GetTestRequest : IGetApiRequest
    {
        private readonly int _id;

        public string Version => "2.0";

        public GetTestRequest (int id)
        {
            _id = id;
        }
        public string GetUrl => $"test-url/get{_id}";
    }
}
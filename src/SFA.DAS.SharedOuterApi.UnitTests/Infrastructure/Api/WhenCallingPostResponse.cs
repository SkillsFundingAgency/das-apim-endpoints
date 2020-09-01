using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.UnitTests.Infrastructure.Api
{
    public class WhenCallingPostResponse
    {
        [Test, AutoData]
        public async Task Then_The_Endpoint_Is_Called_And_Content_Returned(
            string authToken,
            string postContent,
            int id,
            string responseContent,
            TestInnerApiConfiguration config)
        {
            //Arrange
            var azureClientCredentialHelper = new Mock<IAzureClientCredentialHelper>();
            azureClientCredentialHelper.Setup(x => x.GetAccessTokenAsync(config.Identifier)).ReturnsAsync(authToken);
            config.Url = "https://test.local";
            var testObject = JsonConvert.SerializeObject(new TestResponse{MyResponse = responseContent});
            var response = new HttpResponseMessage
            {
                Content = new StringContent(testObject),
                StatusCode = HttpStatusCode.Created
            };
            var getTestRequest = new PostTestRequest(config.Url, id) {BaseUrl = config.Url ,Data = postContent};
            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, getTestRequest.PostUrl, "post");
            var client = new HttpClient(httpMessageHandler.Object);
            var hostingEnvironment = new Mock<IWebHostEnvironment>();
            var clientFactory = new Mock<IHttpClientFactory>();
            clientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
            
            hostingEnvironment.Setup(x => x.EnvironmentName).Returns("Staging");
            var actual = new InternalApiClient<TestInnerApiConfiguration>(clientFactory.Object, config,hostingEnvironment.Object, azureClientCredentialHelper.Object);

            //Act
            var actualResult = await actual.Post<TestResponse>(getTestRequest);

            //Assert
            httpMessageHandler.Protected()
                .Verify<Task<HttpResponseMessage>>(
                    "SendAsync", Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(c =>
                        c.Method.Equals(HttpMethod.Post)
                        && c.RequestUri.AbsoluteUri.Equals(getTestRequest.PostUrl)
                        && c.Headers.Authorization.Scheme.Equals("Bearer")
                        && c.Headers.FirstOrDefault(h=>h.Key.Equals("X-Version")).Value.FirstOrDefault() == "2.0"
                        && c.Headers.Authorization.Parameter.Equals(authToken)),
                    ItExpr.IsAny<CancellationToken>()
                );
            
            actualResult.MyResponse.Should().Be(responseContent);
        }
        
        private class PostTestRequest : IPostApiRequest
        {
            private readonly int _id;

            public string Version => "2.0";

            public PostTestRequest (string baseUrl, int id)
            {
                _id = id;
                BaseUrl = baseUrl;
            }
            public object Data { get; set; }
            public string BaseUrl { get; set; }
            public string PostUrl => $"{BaseUrl}/test-url/get{_id}";
        }

        private class TestResponse
        {
            public string MyResponse { get; set; }
        }
    }
}
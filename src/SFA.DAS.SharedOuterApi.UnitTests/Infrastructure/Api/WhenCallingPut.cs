using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.UnitTests.Infrastructure.Api
{
    public class WhenCallingPut
    {
        [Test, AutoData]
        public async Task Then_The_Endpoint_Is_Called(
            string authToken,
            string postContent,
            int id,
            TestInternalApiConfiguration config)
        {
            //Arrange
            var azureClientCredentialHelper = new Mock<IAzureClientCredentialHelper>();
            azureClientCredentialHelper.Setup(x => x.GetAccessTokenAsync(config.Identifier)).ReturnsAsync(authToken);
            config.Url = "https://test.local";
            var response = new HttpResponseMessage
            {
                Content = new StringContent(""),
                StatusCode = HttpStatusCode.NoContent
            };
            var putTestRequest = new PutTestRequest(config.Url, id) {BaseUrl = config.Url ,Data = postContent};
            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, putTestRequest.PutUrl, "put");
            var client = new HttpClient(httpMessageHandler.Object);
            var hostingEnvironment = new Mock<IWebHostEnvironment>();
            var clientFactory = new Mock<IHttpClientFactory>();
            clientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
            
            hostingEnvironment.Setup(x => x.EnvironmentName).Returns("Staging");
            var actual = new InternalApiClient<TestInternalApiConfiguration>(clientFactory.Object, config,hostingEnvironment.Object, azureClientCredentialHelper.Object);

            //Act
            await actual.Put(putTestRequest);

            //Assert
            httpMessageHandler.Protected()
                .Verify<Task<HttpResponseMessage>>(
                    "SendAsync", Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(c =>
                        c.Method.Equals(HttpMethod.Put)
                        && c.RequestUri.AbsoluteUri.Equals(putTestRequest.PutUrl)
                        && c.Headers.Authorization.Scheme.Equals("Bearer")
                        && c.Headers.FirstOrDefault(h=>h.Key.Equals("X-Version")).Value.FirstOrDefault() == "2.0"
                        && c.Headers.Authorization.Parameter.Equals(authToken)),
                    ItExpr.IsAny<CancellationToken>()
                );
        }
        
        private class PutTestRequest : IPutApiRequest
        {
            private readonly int _id;

            public string Version => "2.0";

            public PutTestRequest(string baseUrl, int id)
            {
                _id = id;
                BaseUrl = baseUrl;
            }
            public object Data { get; set; }
            public string BaseUrl { get; set; }
            public string PutUrl => $"{BaseUrl}/test-url/put{_id}";
        }
    }
}
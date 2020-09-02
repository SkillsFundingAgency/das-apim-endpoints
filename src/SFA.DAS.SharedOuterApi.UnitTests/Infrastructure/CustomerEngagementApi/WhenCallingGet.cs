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
using SFA.DAS.SharedOuterApi.UnitTests.Infrastructure.Api;

namespace SFA.DAS.SharedOuterApi.UnitTests.Infrastructure.CustomerEngagementApi
{
    public class WhenCallingGet
    {
        [Test, AutoData]
        public async Task Then_The_Endpoint_Is_Called(
            int id,
            TestCustomerEngagementApiConfiguration config)
        {
            //Arrange
            config.Url = "https://test.local";
            var response = new HttpResponseMessage
            {
                Content = new StringContent(""),
                StatusCode = HttpStatusCode.Accepted
            };
            var getTestRequest = new GetTestRequest(config.Url, id) {BaseUrl = config.Url };
            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, getTestRequest.GetUrl);
            var client = new HttpClient(httpMessageHandler.Object);
            var hostingEnvironment = new Mock<IWebHostEnvironment>();
            var clientFactory = new Mock<IHttpClientFactory>();
            clientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
            
            hostingEnvironment.Setup(x => x.EnvironmentName).Returns("Staging");
            var actual = new CustomerEngagementApiClient<TestCustomerEngagementApiConfiguration>(clientFactory.Object, config,hostingEnvironment.Object);

            //Act
            await actual.Get<string>(getTestRequest);

            //Assert
            httpMessageHandler.Protected()
                .Verify<Task<HttpResponseMessage>>(
                    "SendAsync", Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(c =>
                        c.Method.Equals(HttpMethod.Get)
                        && c.RequestUri.AbsoluteUri.Equals(getTestRequest.GetUrl)
                        && c.Headers.FirstOrDefault(h=>h.Key.Equals("Ocp-Apim-Subscription-Key")).Value.FirstOrDefault() == config.SubscriptionKey),
                    ItExpr.IsAny<CancellationToken>()
                );
        }

        private class GetTestRequest : IGetApiRequest
        {
            private readonly int _id;

            public string Version => "2.0";

            public GetTestRequest (string baseUrl, int id)
            {
                _id = id;
                BaseUrl = baseUrl;
            }
            public string BaseUrl { get; set; }
            public string GetUrl => $"{BaseUrl}/test-url/get{_id}";
        }
    }
}

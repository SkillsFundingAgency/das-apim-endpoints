using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.UnitTests.Infrastructure.Api
{
    public class WhenCallingGetAll
    {
        [Test, AutoData]
        public async Task Then_The_Endpoint_Is_Called(
            string authToken,
            TestInnerApiConfiguration config)
        {
            //Arrange
            var azureClientCredentialHelper = new Mock<IAzureClientCredentialHelper>();
            azureClientCredentialHelper.Setup(x => x.GetAccessTokenAsync(config.Identifier)).ReturnsAsync(authToken);
            config.Url = "https://test.local";
            var configuration = config;
            var response = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(new List<string>{"string","string"})),
                StatusCode = HttpStatusCode.Accepted
            };
            var getTestRequest = new GetAllTestRequest();
            var expectedUrl = $"{config.Url}{getTestRequest.GetAllUrl}";
            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, expectedUrl);
            var client = new HttpClient(httpMessageHandler.Object);
            var clientFactory = new Mock<IHttpClientFactory>();
            clientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
            
            var hostingEnvironment = new Mock<IWebHostEnvironment>();
            hostingEnvironment.Setup(x => x.EnvironmentName).Returns("Staging");
            var apiClient = new ApiClient<TestInnerApiConfiguration>(clientFactory.Object, configuration,hostingEnvironment.Object, azureClientCredentialHelper.Object);

            //Act
            var actual = await apiClient.GetAll<string>(getTestRequest);

            Assert.IsAssignableFrom<List<string>>(actual);
            //Assert
            httpMessageHandler.Protected()
                .Verify<Task<HttpResponseMessage>>(
                    "SendAsync", Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(c =>
                        c.Method.Equals(HttpMethod.Get)
                        && c.RequestUri.AbsoluteUri.Equals(expectedUrl)
                        && c.Headers.Authorization.Scheme.Equals("Bearer")
                        && c.Headers.Authorization.Parameter.Equals(authToken)),
                    ItExpr.IsAny<CancellationToken>()
                );
        }

        [Test, AutoData]
         public async Task Then_The_Bearer_Token_Is_Not_Added_If_Local(
             TestInnerApiConfiguration config)
         {
             //Arrange
             config.Url = "https://test.local";
             var configuration = config;
             var response = new HttpResponseMessage
             {
                 Content = new StringContent(""),
                 StatusCode = HttpStatusCode.Accepted
             };
             var getTestRequest = new GetAllTestRequest();
             var expectedUrl = $"{config.Url}{getTestRequest.GetAllUrl}";
             var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, expectedUrl);
             var client = new HttpClient(httpMessageHandler.Object);
             var clientFactory = new Mock<IHttpClientFactory>();
             clientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
             
             var hostingEnvironment = new Mock<IWebHostEnvironment>();
             hostingEnvironment.Setup(x => x.EnvironmentName).Returns("Development");
             var actual = new ApiClient<TestInnerApiConfiguration>(clientFactory.Object,configuration,hostingEnvironment.Object, Mock.Of<IAzureClientCredentialHelper>());

             //Act
             await actual.GetAll<string>(getTestRequest);
             
             //Assert
             httpMessageHandler.Protected()
                 .Verify<Task<HttpResponseMessage>>(
                     "SendAsync", Times.Once(),
                     ItExpr.Is<HttpRequestMessage>(c =>
                         c.Method.Equals(HttpMethod.Get)
                         && c.RequestUri.AbsoluteUri.Equals(expectedUrl)
                         && c.Headers.Authorization == null),
                     ItExpr.IsAny<CancellationToken>()
                 );
         }
        
        private class GetAllTestRequest : IGetAllApiRequest
        {
            public string GetAllUrl => "/test-url/get-all";
        }
        
    }
}
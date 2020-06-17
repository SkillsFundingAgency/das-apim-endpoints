using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Domain.Configuration;
using SFA.DAS.FindApprenticeshipTraining.Application.Domain.Interfaces;
using SFA.DAS.FindApprenticeshipTraining.Application.Infrastructure.Api;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Infrastructure.Api
{
    public class WhenCallingGet
    {
        [Test, AutoData]
        public void Then_The_Endpoint_Is_Called(
            string authToken,
            CoursesApiConfiguration config)
        {
            //Arrange
            var azureClientCredentialHelper = new Mock<IAzureClientCredentialHelper>();
            azureClientCredentialHelper.Setup(x => x.GetAccessTokenAsync()).ReturnsAsync(authToken);
            var configuration = new Mock<IOptions<CoursesApiConfiguration>>();
            config.Url = "https://test.local";
            configuration.Setup(x => x.Value).Returns(config);
            var response = new HttpResponseMessage
            {
                Content = new StringContent(""),
                StatusCode = HttpStatusCode.Accepted
            };
            var getTestRequest = new GetTestRequest(config.Url) {BaseUrl = config.Url };
            var httpMessageHandler = SetupMessageHandlerMock(response, getTestRequest.GetUrl);
            var client = new HttpClient(httpMessageHandler.Object);
            var hostingEnvironment = new Mock<IHostingEnvironment>();
            hostingEnvironment.Setup(x => x.EnvironmentName).Returns("Staging");
            var actual = new ApiClient(configuration.Object,client,hostingEnvironment.Object, azureClientCredentialHelper.Object);

            //Act
            actual.Get<string>(getTestRequest);

            //Assert
            httpMessageHandler.Protected()
                .Verify<Task<HttpResponseMessage>>(
                    "SendAsync", Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(c =>
                        c.Method.Equals(HttpMethod.Get)
                        && c.RequestUri.AbsoluteUri.Equals(getTestRequest.GetUrl)
                        && c.Headers.Authorization.Scheme.Equals("Bearer")
                        && c.Headers.Authorization.Parameter.Equals(authToken)),
                    ItExpr.IsAny<CancellationToken>()
                );
        }

        [Test, AutoData]
         public void Then_The_Bearer_Token_Is_Not_Added_If_Local(
             CoursesApiConfiguration config)
         {
             //Arrange
             var configuration = new Mock<IOptions<CoursesApiConfiguration>>();
             config.Url = "https://test.local";
             configuration.Setup(x => x.Value).Returns(config);
             var response = new HttpResponseMessage
             {
                 Content = new StringContent(""),
                 StatusCode = HttpStatusCode.Accepted
             };
             var getTestRequest = new GetTestRequest(config.Url) {BaseUrl = config.Url };
             var httpMessageHandler = SetupMessageHandlerMock(response, getTestRequest.GetUrl);
             var client = new HttpClient(httpMessageHandler.Object);
             var hostingEnvironment = new Mock<IHostingEnvironment>();
             hostingEnvironment.Setup(x => x.EnvironmentName).Returns("Development");
             var actual = new ApiClient(configuration.Object,client,hostingEnvironment.Object, Mock.Of<IAzureClientCredentialHelper>());

             //Act
             actual.Get<string>(getTestRequest);
             
             //Assert
             httpMessageHandler.Protected()
                 .Verify<Task<HttpResponseMessage>>(
                     "SendAsync", Times.Once(),
                     ItExpr.Is<HttpRequestMessage>(c =>
                         c.Method.Equals(HttpMethod.Get)
                         && c.RequestUri.AbsoluteUri.Equals(getTestRequest.GetUrl)
                         && c.Headers.Authorization == null),
                     ItExpr.IsAny<CancellationToken>()
                 );
         }
        
        private class GetTestRequest : IGetApiRequest
        {
            public GetTestRequest (string baseUrl)
            {
                BaseUrl = baseUrl;
            }
            public string BaseUrl { get; set; }
            public string GetUrl => $"{BaseUrl}/test-url/get";
        }
        
        private Mock<HttpMessageHandler> SetupMessageHandlerMock(HttpResponseMessage response, string baseUrl)
        {
            var httpMessageHandler = new Mock<HttpMessageHandler>();
            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(c =>
                        c.Method.Equals(HttpMethod.Get)
                        && c.RequestUri.AbsoluteUri.Equals(baseUrl)),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) => response);
            return httpMessageHandler;
        }
    }
}

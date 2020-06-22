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
using SFA.DAS.FindApprenticeshipTraining.Application.Configuration;
using SFA.DAS.FindApprenticeshipTraining.Application.Infrastructure.Api;
using SFA.DAS.FindApprenticeshipTraining.Application.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Infrastructure.Api
{
    public class WhenCallingGet
    {
        [Test, AutoData]
        public async Task Then_The_Endpoint_Is_Called(
            string authToken,
            int id,
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
            var getTestRequest = new GetTestRequest(config.Url, id) {BaseUrl = config.Url };
            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, getTestRequest.GetUrl);
            var client = new HttpClient(httpMessageHandler.Object);
            var hostingEnvironment = new Mock<IHostingEnvironment>();
            hostingEnvironment.Setup(x => x.EnvironmentName).Returns("Staging");
            var actual = new ApiClient(configuration.Object,client,hostingEnvironment.Object, azureClientCredentialHelper.Object);

            //Act
            await actual.Get<string>(getTestRequest);

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
         public async Task Then_The_Bearer_Token_Is_Not_Added_If_Local(
             int id,
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
             var getTestRequest = new GetTestRequest(config.Url,id) {BaseUrl = config.Url };
             var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, getTestRequest.GetUrl);
             var client = new HttpClient(httpMessageHandler.Object);
             var hostingEnvironment = new Mock<IHostingEnvironment>();
             hostingEnvironment.Setup(x => x.EnvironmentName).Returns("Development");
             var actual = new ApiClient(configuration.Object,client,hostingEnvironment.Object, Mock.Of<IAzureClientCredentialHelper>());

             //Act
             await actual.Get<string>(getTestRequest);
             
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
            private readonly int _id;

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

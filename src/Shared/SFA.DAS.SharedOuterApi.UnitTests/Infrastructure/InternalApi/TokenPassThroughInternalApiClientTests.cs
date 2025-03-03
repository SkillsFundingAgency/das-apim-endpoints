using AutoFixture.NUnit3;
using Moq.Protected;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Infrastructure;
using System;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.UnitTests.Infrastructure.InternalApi
{
    public class TokenPassThroughInternalApiClientTests
    {
        [Test, AutoData]
        public async Task Then_The_Endpoint_Is_Called(
            int id,
            TestTokenPassThroughApiConfiguration config)
        {
            //Arrange
            var tokenValue = Guid.NewGuid().ToString();
            var authToken = $"Bearer {tokenValue}";
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(x => x.HttpContext.Request.Headers["X-Forwarded-Authorization"]).Returns(authToken);
            config.Url = "https://test.local";
            var response = new HttpResponseMessage
            {
                Content = new StringContent("\"test\""),
                StatusCode = HttpStatusCode.Accepted
            };
            var getTestRequest = new GetTestRequest(id);
            var expectedUrl = $"{config.Url}/{getTestRequest.GetUrl}";
            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, expectedUrl);
            var client = new HttpClient(httpMessageHandler.Object);
            var clientFactory = new Mock<IHttpClientFactory>();
            clientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
            var sut = new TokenPassThroughInternalApiClient<TestTokenPassThroughApiConfiguration>(
                clientFactory.Object, 
                config, 
                httpContextAccessor.Object, 
                Mock.Of<ILogger<TokenPassThroughInternalApiClient<TestTokenPassThroughApiConfiguration>>>());

            //Act
            await sut.Get<string>(getTestRequest);

            //Assert
            httpMessageHandler.Protected()
                .Verify<Task<HttpResponseMessage>>(
                    "SendAsync", Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(c =>
                        c.Method.Equals(HttpMethod.Get)
                        && c.RequestUri.AbsoluteUri.Equals(expectedUrl)
                        && c.Headers.Authorization.Scheme.Equals("Bearer")
                        && c.Headers.FirstOrDefault(h => h.Key.Equals("X-Version")).Value.FirstOrDefault() == "2.0"
                        && c.Headers.Authorization.Parameter.Equals(tokenValue)
                       ),
                    ItExpr.IsAny<CancellationToken>()
                );
        }
        
        [Test, AutoData]
        public async Task Then_The_Endpoint_Is_Called_And_Falls_Back_To_Authorisation_Header(
            int id,
            TestTokenPassThroughApiConfiguration config)
        {
            //Arrange
            var tokenValue = Guid.NewGuid().ToString();
            var authToken = $"Bearer {tokenValue}";
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(x => x.HttpContext.Request.Headers.Authorization).Returns(authToken);
            config.Url = "https://test.local";
            var response = new HttpResponseMessage
            {
                Content = new StringContent("\"test\""),
                StatusCode = HttpStatusCode.Accepted
            };
            var getTestRequest = new GetTestRequest(id);
            var expectedUrl = $"{config.Url}/{getTestRequest.GetUrl}";
            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, expectedUrl);
            var client = new HttpClient(httpMessageHandler.Object);
            var clientFactory = new Mock<IHttpClientFactory>();
            clientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
            var sut = new TokenPassThroughInternalApiClient<TestTokenPassThroughApiConfiguration>(
                clientFactory.Object, 
                config, 
                httpContextAccessor.Object, 
                Mock.Of<ILogger<TokenPassThroughInternalApiClient<TestTokenPassThroughApiConfiguration>>>());

            //Act
            await sut.Get<string>(getTestRequest);

            //Assert
            httpMessageHandler.Protected()
                .Verify<Task<HttpResponseMessage>>(
                    "SendAsync", Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(c =>
                        c.Method.Equals(HttpMethod.Get)
                        && c.RequestUri.AbsoluteUri.Equals(expectedUrl)
                        && c.Headers.Authorization.Scheme.Equals("Bearer")
                        && c.Headers.FirstOrDefault(h => h.Key.Equals("X-Version")).Value.FirstOrDefault() == "2.0"
                        && c.Headers.Authorization.Parameter.Equals(tokenValue)
                       ),
                    ItExpr.IsAny<CancellationToken>()
                );
        }

        [Test, AutoData]
        public async Task Then_The_ServiceTokenIsGenerated(
            int id,
            TestTokenPassThroughApiConfiguration config)
        {
            //Arrange
            var tokenValue = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIn0.dozjgNryP4J3jVmNHl0w5N_XgL0n3I9PlFUP0THsR8U";
            var authToken = $"Bearer {tokenValue}";
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(x => x.HttpContext.Request.Headers["Authorization"]).Returns(authToken);
            httpContextAccessor.Setup(x => x.HttpContext.Items).Returns(new Dictionary<object,object?>());
            config.Url = "https://test.local";
            config.BearerTokenSigningKey = "abcdefghijklmnopqrstuv123456789==";
            var response = new HttpResponseMessage
            {
                Content = new StringContent("\"test\""),
                StatusCode = HttpStatusCode.Accepted
            };
            var getTestRequest = new GetTestRequest(id);
            var expectedUrl = $"{config.Url}/{getTestRequest.GetUrl}";
            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, expectedUrl);
            var client = new HttpClient(httpMessageHandler.Object);
            var clientFactory = new Mock<IHttpClientFactory>();
            clientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
            var sut = new TokenPassThroughInternalApiClient<TestTokenPassThroughApiConfiguration>(
                clientFactory.Object,
                config,
                httpContextAccessor.Object,
                Mock.Of<ILogger<TokenPassThroughInternalApiClient<TestTokenPassThroughApiConfiguration>>>());

            //Act
            sut.GenerateServiceToken("Any");
            await sut.Get<string>(getTestRequest);

            //Assert
            httpMessageHandler.Protected()
                .Verify<Task<HttpResponseMessage>>(
                    "SendAsync", Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(c =>
                        c.Method.Equals(HttpMethod.Get)
                        && c.RequestUri.AbsoluteUri.Equals(expectedUrl)
                        && c.Headers.Authorization.Scheme.Equals("Bearer")
                        && c.Headers.FirstOrDefault(h => h.Key.Equals("X-Version")).Value.FirstOrDefault() == "2.0"
                       ),
                    ItExpr.IsAny<CancellationToken>()
                );
        }
    }
}

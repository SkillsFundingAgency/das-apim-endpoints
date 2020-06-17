using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Infrastructure.Api
{
    public static class MessageHandler
    {
        public static Mock<HttpMessageHandler> SetupMessageHandlerMock(HttpResponseMessage response, string baseUrl)
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
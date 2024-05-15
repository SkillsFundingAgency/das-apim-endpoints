using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace SFA.DAS.EmployerRequestApprenticeTraining.MockApis
{
    [ExcludeFromCodeCoverage]
    public class RequestApprenticeTrainingInnerApiBuilder
    {
        private readonly WireMockServer _server;

        public RequestApprenticeTrainingInnerApiBuilder(int port, bool ssl)
        {
            _server = WireMockServer.StartWithAdminInterface(port, ssl);
        }

        public static RequestApprenticeTrainingInnerApiBuilder Create(int port)
        {
            return new RequestApprenticeTrainingInnerApiBuilder(port, true);
        }

        public WireMockServer Build()
        {
            Console.WriteLine($"RequestApprenticeTraining Inner Fake Api Running ({_server.Urls[0]})");
            return _server;
        }

        public RequestApprenticeTrainingInnerApiBuilder WithPing()
        {
            _server
                .Given(
                    Request.Create()
                        .WithPath($"/ping")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                );

            return this;
        }
    }
}
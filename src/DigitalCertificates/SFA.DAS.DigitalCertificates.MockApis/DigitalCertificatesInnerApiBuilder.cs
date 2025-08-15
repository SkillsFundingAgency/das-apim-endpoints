using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace SFA.DAS.DigitalCertificates.MockApis
{
    [ExcludeFromCodeCoverage]
    public class DigitalCertificatesInnerApiBuilder
    {
        private readonly WireMockServer _server;

        public DigitalCertificatesInnerApiBuilder(int port, bool ssl)
        {
            _server = WireMockServer.StartWithAdminInterface(port, ssl);
        }

        public static DigitalCertificatesInnerApiBuilder Create(int port)
        {
            return new DigitalCertificatesInnerApiBuilder(port, true);
        }

        public WireMockServer Build()
        {
            Console.WriteLine($"DigitalCertificates Inner Fake Api Running ({_server.Urls[0]})");
            return _server;
        }

        public DigitalCertificatesInnerApiBuilder WithPing()
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
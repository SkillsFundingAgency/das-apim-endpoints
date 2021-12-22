using System;
using System.Net;
using AutoFixture;
using SFA.DAS.ApprenticePortal.Models;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace SFA.DAS.ApprenticePortal.MockApis
{
    public class ApprenticeAccountsInnerApiBuilder
    {
        private readonly WireMockServer _server;
        private readonly Fixture _fixture;
        private readonly Apprentice _apprentice;

        public ApprenticeAccountsInnerApiBuilder(int port)
        {
            _fixture = new Fixture();
            _server = WireMockServer.StartWithAdminInterface(port, true);
            _apprentice = _fixture.Create<Apprentice>();
        }

        public ApprenticeAccountsInnerApiBuilder WithAnyApprentice()
        {
            _server
                .Given(
                    Request.Create()
                        .WithPath("/apprentices/*")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithBodyAsJson(_apprentice)
                );

            return this;
        }

        public ApprenticeAccountsInnerApiBuilder WithPing()
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



        public static ApprenticeAccountsInnerApiBuilder Create(int port)
            => new ApprenticeAccountsInnerApiBuilder(port);

        public WireMockServer Build()
        {
            Console.WriteLine($"Apprentice Accounts Fake Api Running ({_server.Urls[0]})");
            return _server;
        }
    }
}
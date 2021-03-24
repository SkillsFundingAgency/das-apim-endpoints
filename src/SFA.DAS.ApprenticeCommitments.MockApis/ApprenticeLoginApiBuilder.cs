using System;
using System.Net;
using AutoFixture;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace SFA.DAS.ApprenticeCommitments.MockApis
{
    public class ApprenticeLoginApiBuilder
    {
        private readonly WireMockServer _server;
        private readonly Fixture _fixture;

        public ApprenticeLoginApiBuilder(int port)
        {
            _fixture = new Fixture();
            _server = WireMockServer.StartWithAdminInterface(port, true);
        }

        public static ApprenticeLoginApiBuilder Create(int port)
        {
            return new ApprenticeLoginApiBuilder(port);
        }

        public WireMockServer Build()
        {
            Console.WriteLine($"Apprentice Login Fake Api Running ({_server.Urls[0]})");
            return _server;
        }

        public ApprenticeLoginApiBuilder WithPing()
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

        public ApprenticeLoginApiBuilder WithAnyInvitation()
        {
            _server
                .Given(
                    Request.Create().WithPath($"/invitations/*")
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                );

            return this;
        }
    }
}
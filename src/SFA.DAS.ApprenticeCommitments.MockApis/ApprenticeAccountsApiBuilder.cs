using AutoFixture;
using SFA.DAS.ApprenticeCommitments.Apis.ApprenticeAccountsApi;
using System;
using System.Net;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace SFA.DAS.ApprenticeCommitments.MockApis
{
    public class ApprenticeAccountsApiBuilder
    {
        private readonly WireMockServer _server;
        private readonly Fixture _fixture;
        private readonly Apprentice _apprentice;

        public ApprenticeAccountsApiBuilder(int port)
        {
            _fixture = new Fixture();
            _server = WireMockServer.StartWithAdminInterface(port, true);
            _apprentice = _fixture.Create<Apprentice>();
        }

        public ApprenticeAccountsApiBuilder WithAnyNewApprenticeship()
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

            _server
                .Given(
                    Request.Create().WithPath("/apprenticeships/change").UsingPost()
                )
                .RespondWith(
                    Response.Create().WithStatusCode((int)HttpStatusCode.Accepted)
                );

            return this;
        }

        public static ApprenticeCommitmentsInnerApiBuilder Create(int port)
            => new ApprenticeCommitmentsInnerApiBuilder(port);

        public WireMockServer Build()
        {
            Console.WriteLine($"Apprentice Accounts Fake Api Running ({_server.Urls[0]})");
            return _server;
        }
    }
}
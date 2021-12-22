using System;
using System.Net;
using AutoFixture;
using SFA.DAS.ApprenticePortal.Models;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace SFA.DAS.ApprenticePortal.MockApis
{
    public class ApprenticeCommitmentsInnerApiBuilder
    {
        private readonly WireMockServer _server;
        private readonly Fixture _fixture;
        private readonly GetApprenticeApprenticeshipsResult _existingApprenticeships;

        public ApprenticeCommitmentsInnerApiBuilder(int port)
        {
            _fixture = new Fixture();
            _server = WireMockServer.StartWithAdminInterface(port, true);

            _existingApprenticeships = _fixture.Create<GetApprenticeApprenticeshipsResult>();
        }

        public static ApprenticeCommitmentsInnerApiBuilder Create(int port)
        {
            return new ApprenticeCommitmentsInnerApiBuilder(port);
        }

        public WireMockServer Build()
        {
            Console.WriteLine($"Apprentice Commitments Inner Fake Api Running ({_server.Urls[0]})");
            return _server;
        }

        public ApprenticeCommitmentsInnerApiBuilder WithPing()
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

        public ApprenticeCommitmentsInnerApiBuilder WithExistingApprenticeships()
        {
            _server
                .Given(
                    Request.Create()
                        .WithPath("/apprentices/*/apprenticeships")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                    .WithStatusCode((int)HttpStatusCode.OK)
                    .WithBodyAsJson(_existingApprenticeships)
                );

            return this;
        }
    }
}
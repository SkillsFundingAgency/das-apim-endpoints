using System;
using System.Net;
using AutoFixture;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.TrainingProviderService;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace SFA.DAS.ApprenticeCommitments.MockApis
{
    public class TrainingProviderApiBuilder
    {
        private readonly WireMockServer _server;
        private readonly Fixture _fixture;

        public TrainingProviderApiBuilder(int port)
        {
            _fixture = new Fixture();
            _server = WireMockServer.StartWithAdminInterface(port, true);
        }

        public static TrainingProviderApiBuilder Create(int port)
        {
            return new TrainingProviderApiBuilder(port);
        }

        public WireMockServer Build()
        {
            Console.WriteLine($"Training Provider Fake Api Running  ({_server.Urls[0]})");
            return _server;
        }

        public TrainingProviderApiBuilder WithPing()
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

        public TrainingProviderApiBuilder WithValidSearch(long trainingProviderId)
        {
            var result = _fixture.Build<TrainingProviderResponse>()
                .With(x => x.Ukprn, trainingProviderId)
                .Create();

            var response = new SearchResponse { SearchResults = new TrainingProviderResponse[] { result } };

            _server
                .Given(
                    Request.Create()
                        .WithPath($"/api/v1/search")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithBodyAsJson(response)
                );

            return this;
        }
    }
}
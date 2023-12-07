using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.OpenApi.Writers;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.TrainingProviderService;
using WireMock;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.ResponseProviders;
using WireMock.Server;
using WireMock.Settings;
using WireMock.Types;
using WireMock.Util;
using static SFA.DAS.ApprenticeCommitments.MockApis.CommitmentsV2ApiBuilder;

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

        public TrainingProviderApiBuilder WithAnySearch()
        {
            _server
                .Given(
                    Request.Create()
                    .WithPath("/api/v1/search")
                    .UsingGet()
                )
                .RespondWith(new RoatpSearchResponseProvider(_fixture));

            return this;
        }

        public class RoatpSearchResponseProvider : IResponseProvider
        {
            private readonly Fixture _fixture;

            public RoatpSearchResponseProvider(Fixture fixture)
            {
                _fixture = fixture;
            }

            public Task<(IResponseMessage Message, IMapping Mapping)> ProvideResponseAsync(IMapping mapping, IRequestMessage requestMessage, WireMockServerSettings settings)
            {
                var response = ProvideResponseMessageInternal(requestMessage, settings);
                return Task.FromResult<(IResponseMessage Message, IMapping Mapping)>((response, mapping));
            }

            public ResponseMessage ProvideResponseMessage(RequestMessage requestMessage, WireMockServerSettings settings)
            {
                return ProvideResponseMessageInternal(requestMessage, settings);
            }

            private ResponseMessage ProvideResponseMessageInternal(IRequestMessage requestMessage, WireMockServerSettings settings)
            {
                var searchTerm = requestMessage.Query
                    .FirstOrDefault(param => string.Equals(param.Key, "searchTerm", StringComparison.OrdinalIgnoreCase))
                    .Value[0];

                if (long.TryParse(searchTerm, out long providerId))
                {
                    var trainingProviderResponse = _fixture.Build<TrainingProviderResponse>()
                        .With(x => x.Ukprn, providerId)
                        .With(x => x.TradingName, $"Provider{providerId}")
                        .Create();

                    var response = new SearchResponse { SearchResults = new TrainingProviderResponse[] { trainingProviderResponse } };

                    // Construct response
                    var responseMessage = new ResponseMessage
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                        Headers = new Dictionary<string, WireMockList<string>> { { "Content-Type", new WireMockList<string> { "application/json" } } },
                        BodyData = new BodyData
                        {
                            DetectedBodyType = BodyType.Json,
                            BodyAsJson = response
                        }
                    };

                    return responseMessage;
                }

                return new ResponseMessage() { StatusCode = (int)HttpStatusCode.NoContent };
            }
        }
    }
}
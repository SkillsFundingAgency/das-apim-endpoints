using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Identity.Client;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Assessor;
using WireMock;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.ResponseProviders;
using WireMock.Server;
using WireMock.Settings;
using WireMock.Types;
using WireMock.Util;

namespace SFA.DAS.ApprenticeCommitments.MockApis
{
    public class CommitmentsV2ApiBuilder
    {
        private readonly WireMockServer _server;
        private readonly Fixture _fixture;

        public CommitmentsV2ApiBuilder(int port)
        {
            _fixture = new Fixture();
            _server = WireMockServer.StartWithAdminInterface(port, true);
        }

        public static CommitmentsV2ApiBuilder Create(int port)
        {
            return new CommitmentsV2ApiBuilder(port);
        }

        public WireMockServer Build()
        {
            Console.WriteLine($"Commitments V2 Fake Api Running ({_server.Urls[0]})");
            return _server;
        }

        public CommitmentsV2ApiBuilder WithPing()
        {
            _server
                .Given(
                    Request.Create()
                        .WithPath($"/api/ping")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                );

            return this;
        }

        public CommitmentsV2ApiBuilder WithAValidApprentice(long accountId, long apprenticeshipId, int courseId)
        {
            var response = _fixture.Build<Apis.CommitmentsV2InnerApi.ApprenticeshipResponse>()
                .With(x => x.EmployerAccountId, accountId)
                .With(x => x.CourseCode, courseId.ToString())
                .Without(x=>x.StandardUId)
                .With(x=>x.Email, "valid@email.com")
                .Create();

            _server
                .Given(
                    Request.Create()
                        .WithPath($"/api/apprenticeships/{apprenticeshipId}")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithBodyAsJson(response)
                );

            return this;
        }

        public CommitmentsV2ApiBuilder WithAnyApprenticeship()
        {
            _server
                .Given(
                    Request.Create()
                    .WithPath($"/api/apprenticeships/*")
                    .UsingGet()
                )
                .RespondWith(new CommitmentsV2GetApprenticeshipResponseProvider(_fixture));

            return this;
        }

        public class CommitmentsV2GetApprenticeshipResponseProvider : IResponseProvider
        {
            private readonly Fixture _fixture;

            public CommitmentsV2GetApprenticeshipResponseProvider(Fixture fixture)
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
                // Extract the ApprenticeCommitmentsId from the request path segments
                var apprenticeCommitmentsId = long.Parse(requestMessage.PathSegments[2]);

                var fixedStartDate = new DateTime(2023, 06, 01).Date;
                var fixedEndDate = new DateTime(2026, 06, 01).Date;

                var uln = 20000000 + apprenticeCommitmentsId;

                var response = _fixture.Build<Apis.CommitmentsV2InnerApi.ApprenticeshipResponse>()
                    .With(x => x.Id, apprenticeCommitmentsId)
                    .With(x => x.Uln, uln.ToString())
                    .With(x => x.StartDate, fixedStartDate.AddMinutes(-apprenticeCommitmentsId))
                    .With(x => x.EndDate, fixedEndDate.AddMinutes(apprenticeCommitmentsId))
                    .With(x => x.CourseCode, 100.ToString())
                    .With(x => x.StandardUId, "ST0100_1.0")
                    .With(x => x.ProviderId, apprenticeCommitmentsId)
                    .Create();
                
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
        }
    }
}
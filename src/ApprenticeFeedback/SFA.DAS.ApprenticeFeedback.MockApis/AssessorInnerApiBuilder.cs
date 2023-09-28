using AutoFixture;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WireMock;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.ResponseProviders;
using WireMock.Server;
using WireMock.Settings;
using WireMock.Types;
using WireMock.Util;

namespace SFA.DAS.ApprenticeFeedback.MockApis
{
    public class AssessorInnerApiBuilder
    {
        private readonly WireMockServer _server;
        private readonly Fixture _fixture;

        public AssessorInnerApiBuilder(int port, bool ssl)
        {
            _fixture = new Fixture();
            _server = WireMockServer.StartWithAdminInterface(port, ssl);
        }

        public static AssessorInnerApiBuilder Create(int port)
        {
            return new AssessorInnerApiBuilder(port, true);
        }

        public WireMockServer Build()
        {
            Console.WriteLine($"Assessor Inner Fake Api Running ({_server.Urls[0]})");
            return _server;
        }

        public AssessorInnerApiBuilder WithPing()
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

        public AssessorInnerApiBuilder WithLearner()
        {
            _server
                .Given(
                    Request.Create()
                    .WithPath("/api/v1/learnerdetails/*")
                    .UsingGet()
                )
                .RespondWith(new CustomResponseProvider());

            return this;
        }

        public class CustomResponseProvider : IResponseProvider
        {
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
                var apprenticeCommitmentsId = long.Parse(requestMessage.PathSegments[3]);

                var fixedStartDate = new DateTime(2023, 06, 01).Date;
                var fixedEndDate = new DateTime(2026, 06, 01).Date;

                var response = new GetApprenticeLearnerResponse()
                {
                    ApprenticeshipId = apprenticeCommitmentsId,
                    Ukprn = 1000000 + apprenticeCommitmentsId,
                    Uln = 20000000 + apprenticeCommitmentsId,
                    CompletionStatus = 1,
                    GivenNames = $"Test{apprenticeCommitmentsId}",
                    FamilyName = $"Tester{apprenticeCommitmentsId}",
                    StandardCode = 100,
                    StandardReference = "ST0100",
                    StandardName = "Test Apprenticeship",
                    StandardUId = "ST0100_1.0",
                    LearnStartDate = fixedStartDate.AddMinutes(-apprenticeCommitmentsId),
                    EstimatedEndDate = fixedEndDate.AddMinutes(apprenticeCommitmentsId),
                    PlannedEndDate = fixedEndDate.AddMinutes(apprenticeCommitmentsId),
                    LearnActEndDate = fixedEndDate.AddMinutes(apprenticeCommitmentsId),
                    ProviderName = $"Provider{apprenticeCommitmentsId}"
                };

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
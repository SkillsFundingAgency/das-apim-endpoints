using AutoFixture;
using FluentAssertions;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using System;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "RegistrationFirstSeen")]
    public class RegistrationFirstSeenSteps
    {
        private readonly TestContext _context;
        private RegistrationFirstSeenRequestData _request;
        private Guid _apprenticeId;
        private Fixture _fixture;

        public RegistrationFirstSeenSteps(TestContext context)
        {
            _context = context;
            _fixture = new Fixture();
            _apprenticeId = _fixture.Create<Guid>();
            _request = _fixture.Create<RegistrationFirstSeenRequestData>();

            _context.InnerApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath($"/registrations/{_apprenticeId}/firstseen")
                        .UsingPost()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.Accepted)
                );
        }


        [When(@"the outer api recieves a registration first seen request")]
        public Task WhenTheOuterApiRecievesARegistrationFirstSeenRequest()
        {
            return _context.OuterApiClient.Post($"/registrations/{_apprenticeId}/firstseen", _request);
        }

        [Then(@"the outer api returns an accepted response")]
        public void ThenTheOuterApiReturnsAnAcceptedResponse()
        {
             _context.OuterApiClient.Response.StatusCode.Should().Be(HttpStatusCode.Accepted);
        }
    }
}
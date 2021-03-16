using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "GetApprenticeship")]
    public class GetApprenticeshipSteps
    {
        private readonly TestContext _context;
        private readonly Fixture _fixture = new Fixture();
        private Guid _apprenticeId;
        private long _apprenticeshipId;
        private ApprenticeshipResponse _apprenticeship;

        public GetApprenticeshipSteps(TestContext context)
        {
            _context = context;
            _apprenticeId = _fixture.Create<Guid>();
            _apprenticeshipId = _fixture.Create<int>();
            _apprenticeship = _fixture.Build<ApprenticeshipResponse>()
                .With(x=>x.Id, _apprenticeshipId)
                .Create();
        }

        [Given(@"there is an apprenticeship")]
        public void GivenThereIsAnApprenticeship()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath($"/apprentices/{_apprenticeId}/apprenticeships/{_apprenticeshipId}")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int) HttpStatusCode.OK)
                        .WithBodyAsJson(_apprenticeship)
                );
        }

        [Given("there is no apprenticeship")]
        public void GivenThereIsNoApprenticeship()
        {
        }

        [When(@"the apprenticeship is requested")]
        public async Task WhenTheApprenticeshipIsRequested()
        {
            await _context.OuterApiClient.Get($"/apprentices/{_apprenticeId}/apprenticeships/{_apprenticeshipId}");
        }

        [Then("the result should be OK")]
        public void ThenTheResultShouldBeOK()
        {
            _context.OuterApiClient.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Then(@"the result should contain the anticipated values")]
        public async Task ThenTheResultShouldContainTheAnticipatedValues()
        {
            var content = await _context.OuterApiClient.Response.Content.ReadAsStringAsync();
            var apprenticeship = JsonConvert.DeserializeObject<ApprenticeshipResponse>(content);
            apprenticeship.Should().BeEquivalentTo(_apprenticeship);

        }

        [Then("the result should be NotFound")]
        public void ThenTheResultShouldBeNotFound()
        {
            _context.OuterApiClient.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
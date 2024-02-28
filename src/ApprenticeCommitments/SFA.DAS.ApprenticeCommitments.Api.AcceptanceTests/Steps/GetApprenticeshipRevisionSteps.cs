using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
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
    [Scope(Feature = "GetApprenticeshipRevision")]
    public class GetApprenticeshipRevisionSteps
    {
        private const string ContentType = "application/test-content-type";
        private readonly TestContext _context;
        private readonly Fixture _fixture = new Fixture();
        private Guid _apprenticeId;
        private long _apprenticeshipId;
        private long _revisionId;
        private ApprenticeshipResponse _apprenticeshipRevision;

        public GetApprenticeshipRevisionSteps(TestContext context)
        {
            _context = context;
            _apprenticeId = _fixture.Create<Guid>();
            _apprenticeshipId = _fixture.Create<int>();
            _revisionId = _fixture.Create<int>();
            _apprenticeshipRevision = _fixture.Build<ApprenticeshipResponse>()
                .With(x=>x.Id, _apprenticeshipId)
                .Create();
        }

        [Given(@"there is an apprenticeship revision")]
        public void GivenThereIsAnApprenticeshipRevision()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath($"/apprentices/{_apprenticeId}/apprenticeships/{_apprenticeshipId}/revisions/{_revisionId}")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int) HttpStatusCode.OK)
                        .WithBodyAsJson(_apprenticeshipRevision)
                        .WithHeader("Content-Type", ContentType)
                );
        }

        [Given("there is no apprenticeship revision")]
        public void GivenThereIsNoRevision()
        {
        }

        [When(@"an explicit apprenticeship revision is requested")]
        public async Task WhenAnExplicitApprenticeshipRevisionIsRequested()
        {
            await _context.OuterApiClient.Get($"/apprentices/{_apprenticeId}/apprenticeships/{_apprenticeshipId}/revisions/{_revisionId}");
        }

        [When(@"the wrong apprenticeship revision is requested")]
        public async Task WhenTheWrongApprenticeshipRevisionIsRequested()
        {
            await _context.OuterApiClient.Get($"/apprentices/{_apprenticeId}/apprenticeships/{_apprenticeshipId}/revisions/-199");
        }


        [Then("the result should be OK")]
        public void ThenTheResultShouldBeOK()
        {
            _context.OuterApiClient.Response.Should().Be200Ok();
        }

        [Then("the result should have the correct Content-Type")]
        public void ThenTheResultShouldBeJson()
        {
            _context.OuterApiClient.Response
                .Should().HaveHeader("Content-Type")
                .And.Match(ContentType);
        }

        [Then(@"the result should contain the anticipated values")]
        public async Task ThenTheResultShouldContainTheAnticipatedValues()
        {
            var content = await _context.OuterApiClient.Response.Content.ReadAsStringAsync();
            var apprenticeship = JsonConvert.DeserializeObject<ApprenticeshipResponse>(content);
            apprenticeship.Should().BeEquivalentTo(_apprenticeshipRevision);
        }

        [Then("the result should be NotFound")]
        public void ThenTheResultShouldBeNotFound()
        {
            _context.OuterApiClient.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
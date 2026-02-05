using AutoFixture;
using FluentAssertions;
using SFA.DAS.ApprenticeCommitments.Apis.ApprenticeAccountsApi;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "GetApprentice")]
    public class GetApprenticeSteps
    {
        private readonly TestContext _context;
        private readonly Fixture _fixture = new Fixture();
        private Apprentice _apprentice;

        public GetApprenticeSteps(TestContext context)
        {
            _context = context;
            _apprentice = _fixture.Create<Apprentice>();
        }

        [Given("there is an apprentice")]
        public void GivenThereIsAnApprenticeship()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath($"/apprentices/{_apprentice.ApprenticeId}")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithBodyAsJson(_apprentice));
        }

        [Given("there is no apprenticeship")]
        public void GivenThereIsNoApprenticeship()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath("/apprentices/*")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.NotFound));
        }

        [Given("there is a server error")]
        public void GivenThereIsAServerError()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath("/apprentices/*")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.InternalServerError)
                        .WithBodyAsJson(new[] { new { Parameter = "something", Error = "went wrong" } }));
        }

        [When("the apprentice is requested")]
        public async Task WhenTheApprenticeshipOverviewIsRequested()
        {
            await _context.OuterApiClient.Get($"/apprentices/{_apprentice.ApprenticeId}");
        }

        [Then("the result should contain the apprentice data")]
        public void ThenTheResultShouldContainTheApprentice()
        {
            _context.OuterApiClient.Response
                .Should().Be200Ok().And.BeAs(_apprentice);
        }

        [Then("the result should be NotFound")]
        public void ThenTheResultShouldBeNotFound()
        {
            _context.OuterApiClient.Response.Should().Be404NotFound();
        }

        [Then("the result should be the error")]
        public void ThenTheResultShouldBeTheError()
        {
            _context.OuterApiClient.Response
                .Should().Be500InternalServerError()
                .And.BeAs(new[] { new { Parameter = "something", Error = "went wrong" } });
        }
    }
}
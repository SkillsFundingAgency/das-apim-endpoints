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
    [Scope(Feature = "CurrentApprenticeship")]
    public class CurrentApprenticeshipSteps
    {
        private readonly TestContext _context;
        private readonly Fixture _fixture = new Fixture();
        private Guid _apprenticeId;
        private CurrentApprenticeshipResponse _apprenticeship;

        public CurrentApprenticeshipSteps(TestContext context)
            => _context = context;

        [Given("an apprentice has registered")]
        public void GivenAnApprenticeHasRegistered()
        {
            _apprenticeId = _fixture.Create<Guid>();
        }

        [Given("there is a current apprenticeship")]
        public void GivenThereIsACurrentApprenticeship()
        {
            _apprenticeship = _fixture.Create<CurrentApprenticeshipResponse>();

            _context.InnerApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath($"/apprentices/{_apprenticeId}/currentapprenticeship")
                        .UsingGet()
                      )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonConvert.SerializeObject(_apprenticeship))
                            );
        }

        [Given("there is no current apprenticeship")]
        public void GivenThereIsNoCurrentApprenticeship()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath($"/apprentices/{_apprenticeId}/currentapprenticeship")
                        .UsingGet()
                      )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.NotFound)
                );
        }

        [When("the current apprenticeship overview is requested")]
        public async Task WhenTheCurrentApprenticeshipOverviewIsRequested()
        {
            await _context.OuterApiClient
                .Get($"apprentices/{_apprenticeId}/currentapprenticeship");
        }

        [Then("the result should be OK")]
        public void ThenTheResultShouldBeOK()
        {
            _context.OuterApiClient.Response.StatusCode
                .Should().Be(HttpStatusCode.OK);
        }

        [Then("the result should be NotFound")]
        public void ThenTheResultShouldBeNotFound()
        {
            _context.OuterApiClient.Response.StatusCode
                .Should().Be(HttpStatusCode.NotFound);
        }

        [Then("and the current apprenticeship is returned")]
        public async Task ThenAndTheCurrentApprenticeshipIsReturned()
        {
            var content = await _context.OuterApiClient
                .Response.Content.ReadAsStringAsync();

            var result = JsonConvert
                .DeserializeObject<CurrentApprenticeshipResponse>(content);
            
            result.Should().BeEquivalentTo(_apprenticeship);
        }
    }
}
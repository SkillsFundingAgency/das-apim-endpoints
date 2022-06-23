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
    [Scope(Feature = "GetApprenticeships")]
    public class GetApprenticeshipsSteps
    {
        private readonly TestContext _context;
        private readonly Fixture _fixture = new Fixture();
        private Guid _apprenticeId;
        private ApprenticeshipResponse _apprenticeship;

        private string ApprenticeshipsApiRoot => $"/apprentices/{_apprenticeId}/apprenticeships";

        public GetApprenticeshipsSteps(TestContext context)
        {
            _context = context;
            _apprenticeId = _fixture.Create<Guid>();
            _apprenticeship = _fixture.Create<ApprenticeshipResponse>();
        }

        [Given("there is an apprenticeship")]
        public void GivenThereIsAnApprenticeship()
        {
            var getApprenticeshipsResponse = new[]
            {
                new ApprenticeshipResponse
                {
                    Id = _apprenticeship.Id,
                }
            };

            _context.InnerApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath(ApprenticeshipsApiRoot)
                        .UsingGet()
                      )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonConvert.SerializeObject(getApprenticeshipsResponse))
                            );

            _context.InnerApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath($"{ApprenticeshipsApiRoot}/{_apprenticeship.Id}")
                        .UsingGet()
                      )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonConvert.SerializeObject(_apprenticeship))
                            );
        }

        [Given("there is no apprenticeship")]
        public void GivenThereIsNoApprenticeship()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath($"{ApprenticeshipsApiRoot}/*")
                        .UsingGet()
                      )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.NotFound)
                            );
        }

        [When(@"the list of apprenticeships is requested")]
        public async Task WhenTheListOfApprenticeshipsIsRequested()
        {
            await _context.OuterApiClient
                .Get(ApprenticeshipsApiRoot);
        }

        [When("the apprenticeship overview is requested")]
        public async Task WhenTheApprenticeshipOverviewIsRequested()
        {
            await _context.OuterApiClient
                .Get($"{ApprenticeshipsApiRoot}/{_apprenticeship.Id}");
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

        [Then(@"the result should contain the apprenticeship")]
        public async Task ThenTheResultShouldContainTheApprenticeship()
        {
            var content = await _context.OuterApiClient
                .Response.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<List<ApprenticeshipResponse>>(content);
            response.Should().BeEquivalentTo(new[]
            {
                new { _apprenticeship.Id }
            });
        }

        [Then("the apprenticeship is returned")]
        public async Task ThenAndTheApprenticeshipIsReturned()
        {
            var content = await _context.OuterApiClient
                .Response.Content.ReadAsStringAsync();

            var result = JsonConvert
                .DeserializeObject<ApprenticeshipResponse>(content);

            result.Should().BeEquivalentTo(_apprenticeship);
        }
    }
}
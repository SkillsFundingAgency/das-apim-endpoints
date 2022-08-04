using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
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
    [Scope(Feature = "GetMyApprenticeship")]
    public class GetMyApprenticeshipSteps
    {
        private const string ContentType = "application/test-content-type";
        private readonly TestContext _context;
        private readonly Fixture _fixture = new Fixture();
        private Guid _apprenticeId;
        private long _apprenticeshipId;
        private ApprenticeshipResponse _apprenticeship;

        public GetMyApprenticeshipSteps(TestContext context)
        {
            _context = context;
            _apprenticeId = _fixture.Create<Guid>();
            _apprenticeshipId = _fixture.Create<int>();
            _apprenticeship = _fixture.Build<ApprenticeshipResponse>()
                .With(x=>x.Id, _apprenticeshipId)
                .Create();
        }

        [Given(@"there is a confirmed apprenticeship")]
        public void GivenThereIsAnApprenticeship()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath($"/apprentices/{_apprenticeId}/apprenticeships/{_apprenticeshipId}/confirmed/latest")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int) HttpStatusCode.OK)
                        .WithBodyAsJson(_apprenticeship)
                        .WithHeader("Content-Type", ContentType)
                );
        }

        [Given("there is no confirmed apprenticeship")]
        public void GivenThereIsNoApprenticeship()
        {
        }

        [When(@"the latest confirmed apprenticeship is requested")]
        public async Task WhenTheApprenticeshipIsRequested()
        {
            await _context.OuterApiClient.Get($"/apprentices/{_apprenticeId}/apprenticeships/{_apprenticeshipId}/confirmed/latest");
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
            apprenticeship.Should().BeEquivalentTo(_apprenticeship);
        }

        [Then("the result should be NotFound")]
        public void ThenTheResultShouldBeNotFound()
        {
            _context.OuterApiClient.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
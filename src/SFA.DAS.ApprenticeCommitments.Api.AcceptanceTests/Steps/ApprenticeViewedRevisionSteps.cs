using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "ApprenticeViewedRevision")]
    public class ApprenticeViewedRevisionSteps
    {
        private readonly TestContext _context;
        private readonly Fixture _fixture = new Fixture();
        private Guid _apprenticeId;
        private long _apprenticeshipId;
        private long _revisionId;
        private JsonPatchDocument<RevisionPatch> _change;
        private DateTime? _viewedOn;

        public ApprenticeViewedRevisionSteps(TestContext context)
        {
            _context = context;
            _apprenticeId = _fixture.Create<Guid>();
            _apprenticeshipId = _fixture.Create<long>();
            _revisionId = _fixture.Create<long>();
        }


        [Given(@"the inner api is ready")]
        public void GivenTheInnerApiIsReady()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath($"/apprentices/{_apprenticeId}/apprenticeships/{_apprenticeshipId}/revisions/{_revisionId}")
                        .UsingPatch()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                );
        }


        [When(@"the last viewed is set")]
        public async Task WhenTheLastViewedIsSet()
        {
            _viewedOn = DateTime.UtcNow;
            _change = new JsonPatchDocument<RevisionPatch>().Replace(a => a.LastViewed, _viewedOn);
            _context.OuterApiClient.Response = await _context.OuterApiClient.Client.PatchAsync($"/apprentices/{_apprenticeId}/apprenticeships/{_apprenticeshipId}/revisions/{_revisionId}", _change.GetStringContent());
        }

        [Then(@"the last viewed date and time should have been passed to inner api")]
        public void ThenTheLastViewedDateAndTimeShouldHaveBeenPassedToInnerApi()
        {
            var logs = _context.InnerApi.MockServer.LogEntries;
            logs.Should().HaveCount(1);

            var innerApiRequest = JsonConvert.DeserializeObject<JsonPatchDocument<RevisionPatch>>(
                logs.First().RequestMessage.Body);

            innerApiRequest.Should().NotBeNull();
            innerApiRequest.Operations.Should().ContainEquivalentOf(new
            {
                path = "/LastViewed",
                op = "replace",
                value = _viewedOn,
            });
        }

        [Then("the result should be OK")]
        public void ThenTheResultShouldBeOK()
        {
            _context.OuterApiClient.Response.Should().Be200Ok();
        }
    }
}
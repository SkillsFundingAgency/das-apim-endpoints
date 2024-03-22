using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.ApprenticeCommitments.Apis.CommitmentsV2InnerApi;
using System;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Steps
{
    [Binding]
    public class ConfirmApprenticeshipSteps
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly TestContext _context;
        private string _apiPath;
        private object _command;

        public string ApprenticeshipApiPath =>
            $"/apprentices/{_fixture.Create<Guid>()}/apprenticeships/{_fixture.Create<long>()}/revisions/{_fixture.Create<long>()}/confirmations";

        public ConfirmApprenticeshipSteps(TestContext context)
        {
            _context = context;
        }

        [Given("the confirmation `(.*)`")]
        public void GivenDataForConfirmation(string json)
        {
            var t = typeof(Confirmations);
            _command = JsonConvert.DeserializeObject(json, t);
            _apiPath = ApprenticeshipApiPath;
        }

        [Given("the inner API will accept the confirmation")]
        public void GivenTheInnerAPIWillAcceptTheConfirmation()
        {
            _context.InnerApi.MockServer
                .Given(Request.Create().UsingPost())
                .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.OK));
        }

        [When("we confirm the aspect")]
        public async Task WhenWeConfirmTheTrainingProvider()
        {
            await _context.OuterApiClient.Patch(_apiPath, _command);
        }

        [Then("return an ok response")]
        public void ThenReturnAnOkResponse()
        {
            _context.OuterApiClient.Response.Should().Be200Ok();
        }

        [Then("confirm the details with the inner API")]
        public void ThenConfirmTheApprenticeshipDetailsWithTheInnerAPI()
        {
            var request = _context.InnerApi.MockServer.LogEntries
                .Should().Contain(x => x.RequestMessage.Path == _apiPath).Which;

            var innerApiRequest = JsonConvert.DeserializeObject(
                request.RequestMessage.Body, _command.GetType());

            innerApiRequest.Should().BeEquivalentTo(_command);
        }
    }
}
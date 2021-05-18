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
    public class ConfirmApprenticeshipSteps
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly TestContext _context;
        private string _apiPath;
        private object _command;

        public object ApprenticeshipApiPath =>
            $"/apprentices/{_fixture.Create<Guid>()}/apprenticeships/{_fixture.Create<long>()}/{_fixture.Create<long>()}";

        public ConfirmApprenticeshipSteps(TestContext context)
        {
            _context = context;
        }

        [Given("(.*) containing `(.*)` for (.*)")]
        public void GivenDataForConfirmation(string typename, string json, string path)
        {
            var t = Type.GetType(typename);
            _command = JsonConvert.DeserializeObject(json, t);
            _apiPath = $"{ApprenticeshipApiPath}/{path.ToLower()}";
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
            await _context.OuterApiClient.Post(_apiPath, _command);
        }

        [When(@"we confirm the roles and responsibilities")]
        public async Task WhenWeConfirmTheRolesAndResponsibilities()
        {
            await _context.OuterApiClient.Post(
                $"/apprentices/{Guid.NewGuid()}/apprenticeships/{1234}/rolesandresponsibilitiesconfirmation",
                _command);
        }

        [Then("return an ok response")]
        public void ThenReturnAnOkResponse()
        {
            _context.OuterApiClient.Response.StatusCode.Should().Be(HttpStatusCode.OK);
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
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
    public class ConfirmApprenticeshipSteps
    {
        private readonly TestContext _context;
        private object _command;
        private Fixture _fixture;

        public ConfirmApprenticeshipSteps(TestContext context)
        {
            _context = context;
            _fixture = new Fixture();
        }

        [Given("a Training Provider confirmation")]
        public void GivenATrainingProviderConfirmation()
        {
            _command = new TrainingProviderConfirmationRequestData
            {
                TrainingProviderCorrect = true
            };
        }

        [Given("the inner API will accept the confirmation")]
        public void GivenTheInnerAPIWillAcceptTheConfirmation()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath("/apprentices/*/apprenticeships/*/trainingproviderconfirmation")
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK));
        }

        [When("we confirm the training provider")]
        public async Task WhenWeConfirmTheTrainingProvider()
        {
            await _context.OuterApiClient.Post(
                $"/apprentices/{Guid.NewGuid()}/apprenticeships/{1234}/trainingproviderconfirmation",
                _command);
        }

        [Then("return an ok response")]
        public void ThenReturnAnOkResponse()
        {
            _context.OuterApiClient.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
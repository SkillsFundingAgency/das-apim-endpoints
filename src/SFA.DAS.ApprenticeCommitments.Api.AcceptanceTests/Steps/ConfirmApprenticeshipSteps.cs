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

        [Given("an Employer confirmation is requested")]
        public void GivenAEmployerConfirmationIsRequested()
        {
            _command = new EmployerConfirmationRequestData()
            {
                EmployerCorrect = true
            };
        }

        [Given("an Apprenticeship Details confirmation is requested")]
        public void GivenAnApprenticeshipDetailsConfirmationIsRequested()
        {
            _command = new ApprenticeshipDetailsConfirmationRequestData()
            {
                ApprenticeshipDetailsCorrect = true
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

            _context.InnerApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath("/apprentices/*/apprenticeships/*/employerconfirmation")
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK));

            _context.InnerApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath("/apprentices/*/apprenticeships/*/apprenticeshipdetailsconfirmation")
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

        [When("we confirm the employer")]
        public async Task WhenWeConfirmTheEmployer()
        {
            await _context.OuterApiClient.Post(
                $"/apprentices/{Guid.NewGuid()}/apprenticeships/{1234}/employerconfirmation",
                _command);
        }

        [When("we confirm the apprenticeship")]
        public async Task WhenWeConfirmTheApprenticeship()
        {
            await _context.OuterApiClient.Post(
                $"/apprentices/{Guid.NewGuid()}/apprenticeships/{1234}/apprenticeshipdetailsconfirmation",
                _command);
        }

        [Then("return an ok response")]
        public void ThenReturnAnOkResponse()
        {
            _context.OuterApiClient.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
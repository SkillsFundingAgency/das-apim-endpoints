using AutoFixture;
using FluentAssertions;
using SFA.DAS.ApprenticeCommitments.Application.Commands.ChangeEmailAddress;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "ChangeEmailAddress")]
    public class ChangeEmailAddressSteps
    {
        private readonly TestContext _context;
        private ChangeEmailAddressCommand _command;
        private Fixture _fixture;

        public ChangeEmailAddressSteps(TestContext context)
        {
            _context = context;
            _fixture = new Fixture();
        }

        [Given(@"the requested change is valid")]
        public void GivenAnApprenticeExists()
        {
            _command = _fixture.Create<ChangeEmailAddressCommand>();
        }

        [Given(@"the inner API will accept the change")]
        public void GivenTheInnerApiWillAcceptTheChange()
        {
            _context.InnerApi.MockServer
               .Given(
                   Request.Create()
                       .WithPath($"/apprentices/{_command.ApprenticeId}/email")
                       .UsingPost()
               )
               .RespondWith(
                   Response.Create()
                       .WithStatusCode((int)HttpStatusCode.OK)
                       );
        }

        [When(@"we change the apprentice's email address")]
        public Task WhenWeConfirmTheIdentityAndVerifyRegistration()
        {
            return _context.OuterApiClient.Post($"/apprentices/{_command.ApprenticeId}/email", _command);
        }

        [Then(@"return an ok response")]
        public void ThenReturnAnOkResponse()
        {
            _context.OuterApiClient.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
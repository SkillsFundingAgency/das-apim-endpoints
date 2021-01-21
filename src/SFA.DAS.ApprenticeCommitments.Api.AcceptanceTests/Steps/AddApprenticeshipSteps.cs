using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using SFA.DAS.ApprenticeCommitments.Application.Commands.CreateApprenticeship;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "AddApprenticeship")]
    public class AddApprenticeshipSteps
    {
        private readonly TestContext _context;
        private HttpResponseMessage _response;
        private CreateApprenticeshipCommand _request;
        public AddApprenticeshipSteps(TestContext context)
        {
            _context = context;
        }

        [Given(@"apprenticeship details are valid")]
        public void GivenApprenticeshipDetailsAreValid()
        {
            _request = new CreateApprenticeshipCommand
            {
                ApprenticeshipId = 1020,
                Email = "Test@Test.com"
            };
        }

        [Given(@"the inner api is ready")]
        public void GivenTheInnerApiIsReady()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/apprenticeships")
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.Accepted)
                );
        }

        [When(@"the apprenticeship is saved")]
        public async Task WhenTheApprenticeshipIsSaved()
        {
            await _context.OuterApiClient.Post("apprenticeships", _request);
        }

        [Then(@"the result should be Accepted")]
        public void ThenTheResultShouldBeAccepted()
        {
            _context.OuterApiClient.Response.StatusCode.Should().Be(HttpStatusCode.Accepted);
        }
    }
}
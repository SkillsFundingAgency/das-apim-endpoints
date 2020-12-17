using AutoFixture;
using FluentAssertions;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "PausePayments")]
    public class PausePaymentsSteps
    {
        private readonly TestContext _context;
        private PostPausePaymentsRequest _request;
        private HttpResponseMessage _response;
        private HttpStatusCode _innerResponseStatusCode;
        private readonly Fixture _fixture;

        public PausePaymentsSteps(TestContext context)
        {
            _fixture = new Fixture();
            _context = context;
        }

        [Given(@"the caller wants to pause/resume payments for an apprenticeship incentive")]
        public void GivenTheCallerWantsToPauseResumePayments()
        {
            _request = new PostPausePaymentsRequest(
                new PausePaymentsRequest
                {
                    Action = PausePaymentsAction.Pause,
                    AccountLegalEntityId = _fixture.Create<long>(),
                    ULN = _fixture.Create<long>(),
                    ServiceRequest = _fixture.Create<ServiceRequest>()
                });
        }

        [Given(@"the Employer Incentives Api receives request")]
        public void GivenTheEmployerIncentivesApiShouldReceiveTheRequest()
        {
            _innerResponseStatusCode = HttpStatusCode.OK;

            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/pause-payments")
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)_innerResponseStatusCode)                        
                );
        }

        [When(@"the Outer Api receives the Pause Payments request")]
        public async Task WhenTheOuterApiReceivesThePausePaymentsRequest()
        {
           _response = await  _context.OuterApiClient.PostAsJsonAsync($"pause-payments", _request);
        }

        [Then(@"the response of OK is returned")]
        public void ThenReturnAcceptedToTheCaller()
        {
            _response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }
}

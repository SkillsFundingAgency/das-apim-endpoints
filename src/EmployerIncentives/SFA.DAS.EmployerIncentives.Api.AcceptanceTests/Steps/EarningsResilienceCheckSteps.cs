using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "EarningsResilienceCheck")]
    public class EarningsResilienceCheckSteps
    {
        private readonly TestContext _context;
        private HttpResponseMessage _response;

        public EarningsResilienceCheckSteps(TestContext testContext)
        {
            _context = testContext;
        }

        [Given(@"the caller wants to perform an earnings resilience check")]
        public void GivenTheCallerWantsToPerformAnEarningsResilienceCheck()
        {
            
        }

        [Given(@"the Employer Incentives Api receives the earnings resilience check request")]
        public void GivenTheEmployerIncentivesApiReceivesTheEarningsResilienceCheckRequest()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/earnings-resilience-check")
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                );
        }

        [When(@"the Outer Api receives the earnings resilience check request")]
        public async Task WhenTheOuterApiReceivesTheEarningsResilienceCheckRequest()
        {
            _response = await _context.OuterApiClient.PostAsync($"earnings-resilience-check", new StringContent(string.Empty));
        }

        [Then(@"the response code of Ok is returned")]
        public void ThenTheResponseCodeOfOkIsReturned()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

    }
}

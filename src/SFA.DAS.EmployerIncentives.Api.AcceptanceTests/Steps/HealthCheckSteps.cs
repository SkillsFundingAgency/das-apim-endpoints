using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using SFA.DAS.EmployerIncentives.Services;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "HealthChecks")]

    public class HealthCheckSteps
    {
        private readonly TestContext _context;
        private HttpResponseMessage _response;

        public HealthCheckSteps(TestContext context)
        {
            _context = context;
        }

        [Given(@"the Employer Incentives Inner Api is ready and healthy")]
        public void GivenTheEmployerIncentivesInnerApiIsReadyAndHealthy()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/health")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "plain/text")
                        .WithBody("Healthy")
                );
        }

        [Given(@"the Commitments Inner Api is ready and healthy")]
        public void GivenTheCommitmentsInnerApiIsReadyAndHealthy()
        {
            var response = new CommitmentsV2HealthResponse {Status = "Healthy"};

            _context.CommitmentsV2InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/health")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonSerializer.Serialize(response))
                );
        }
        
        [When(@"I ping the Outer Api")]
        public async Task WhenIPingTheOuterApi()
        {
            _response = await _context.OuterApiClient.GetAsync($"/ping");
        }

        [Then(@"the result should show healthy")]
        public async Task ThenTheResultShouldShowHealthy()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.OK);
            var body = await _response.Content.ReadAsStringAsync();
            body.Should().Be("Healthy");
        }
    }
}

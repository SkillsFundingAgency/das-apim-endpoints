using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
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

        [Given(@"the Employer Incentives Inner Api is ready and (.*)")]
        public void GivenTheEmployerIncentivesInnerApiIsReadyAnd(string status)
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/ping")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)StatusCodeFromDescription(status))
                );
        }

        [Given(@"the Commitments Inner Api is ready and (.*)")]
        public void GivenTheCommitmentsInnerApiIsReadyAnd(string status)
        {
            _context.CommitmentsV2InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/api/ping")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)StatusCodeFromDescription(status))
                );
        }

        [Given(@"the Accounts Api is ready and (.*)")]
        public void GivenTheAccountsApiIsReadyAnd(string status)
        {
            _context.AccountsApi.MockServer
                .Given(
                    Request.Create().WithPath($"/ping")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)StatusCodeFromDescription(status))
                );
        }

        [Given(@"the Finance Api is ready and (.*)")]
        public void GivenTheFinanceApiIsReadyAnd(string status)
        {
            _context.FinanceApi.MockServer
                .Given(
                    Request.Create().WithPath($"/finance/heartbeat")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)StatusCodeFromDescription(status))
                );
        }

        [When(@"I ping the Outer Api")]
        public async Task WhenIPingTheOuterApi()
        {
            _response = await _context.OuterApiClient.GetAsync($"/ping");
        }

        [When(@"I call the health endpoint of the Outer Api")]
        public async Task WhenICallTheHealthEndpointOfTheOuterApi()
        {
            _response = await _context.OuterApiClient.GetAsync($"/health");
        }

        [Then(@"the result should show (.*)")]
        public async Task ThenTheResultShouldShow(string status)
        {
            var json = await _response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<HealthResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            result.Should().NotBeNull();
            result.Status.Should().Be(status);
        }

        [Then(@"the result should return Ok status")]
        public void ThenTheResultShouldReturnOkStatus()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        public HttpStatusCode StatusCodeFromDescription(string status)
            =>
                (status == "Healthy")
                    ? HttpStatusCode.OK
                    : HttpStatusCode.InternalServerError;

        public class HealthResponse
        {
            public string Status { get; set; }
        }
    }
}

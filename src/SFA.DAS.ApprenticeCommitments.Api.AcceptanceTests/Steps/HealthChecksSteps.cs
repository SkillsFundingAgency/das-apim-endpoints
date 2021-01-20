using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "HealthChecks")]
    public class HealthChecksSteps : IDisposable
    {
        private readonly TestContext _context;
        private HttpResponseMessage _response;

        public HealthChecksSteps(TestContext context)
        {
            _context = context;
        }

        [When(@"I ping the Outer Api")]
        public async Task WhenIPingTheOuterApi()
        {
            _response = await _context.OuterApiClient.GetAsync("ping");
        }

        [Then(@"the result should return Ok status")]
        public void ThenTheResultShouldReturnOkStatus()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [When(@"I call the health endpoint of the Outer Api")]
        public async Task WhenICallTheHealthEndpointOfTheOuterApi()
        {
            _response = await _context.OuterApiClient.GetAsync("health");
        }

        [Given(@"the Apprentice Commitments Inner Api is ready and (.*)")]
        public void GivenTheApprenticeCommitmentsInnerApiIsReadyAnd(string status)
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

        [Then(@"the result should show (.*)")]
        public async Task ThenTheResultShouldShow(string status)
        {
            var json = await _response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<HealthResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            result.Should().NotBeNull();
            result.Status.Should().Be(status);
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

        public void Dispose()
        {
            _response?.Dispose();
        }
    }
}
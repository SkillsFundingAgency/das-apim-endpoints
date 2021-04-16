using System.Net;
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
    public class HealthChecksSteps
    {
        private readonly TestContext _context;

        public HealthChecksSteps(TestContext context)
        {
            _context = context;
        }

        [When(@"I ping the Outer Api")]
        public async Task WhenIPingTheOuterApi()
        {
            await _context.OuterApiClient.Get("ping");
        }

        [Then(@"the result should return Ok status")]
        public void ThenTheResultShouldReturnOkStatus()
        {
            _context.OuterApiClient.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [When(@"I call the health endpoint of the Outer Api")]
        public async Task WhenICallTheHealthEndpointOfTheOuterApi()
        { 
            await _context.OuterApiClient.Get("health");
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

        [Given(@"the Apprentice Login Api is ready and (.*)")]
        public void GivenTheApprenticeLoginApiIsReadyAnd(string status)
        {
            _context.LoginApi.MockServer
                .Given(
                    Request.Create().WithPath($"/ping")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)StatusCodeFromDescription(status))
                );
        }


        [Given(@"the Commitments V2 Api is ready and (.*)")]
        public void GivenTheCommitmentsVApiIsReadyAndHealthy(string status)
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

        [Given(@"the Training Provider Api is ready and (.*)")]
        public void GivenTheTrainingProviderApiIsReadyAndHealthy(string status)
        {
            _context.TrainingProviderInnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/ping")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)StatusCodeFromDescription(status))
                );
        }


        [Given("the Courses Api is ready and (.*)")]
        public void GivenTheCoursesApiIsReadyAndHealthy(string status)
        {
            _context.CoursesInnerApi.MockServer
                .Given(
                    Request.Create().WithPath("/ping")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)StatusCodeFromDescription(status))
                );
        }

        [Then("the result should show (.*)")]
        public async Task ThenTheResultShouldShow(string status)
        {
            var json = await _context.OuterApiClient.Response.Content.ReadAsStringAsync();
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
    }
}
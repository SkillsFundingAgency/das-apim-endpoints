using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.RecalculateEarnings;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "RecalculateEarnings")]
    public class RecalculateEarningsSteps
    {
        private readonly TestContext _context;
        private HttpResponseMessage _response;
        private RecalculateEarningsRequest _request;
        private Fixture _fixture;

        public RecalculateEarningsSteps(TestContext testContext)
        {
            _context = testContext;
            _fixture = new Fixture();
        }

        [Given(@"the caller wants to recalculate earnings")]
        public void GivenTheCallerWantsToRecalculateEarnings()
        {
            _request = _fixture.Build<RecalculateEarningsRequest>()
                .With(x => x.IncentiveLearnerIdentifiers,
                    _fixture.CreateMany<IncentiveLearnerIdentifierDto>(5).ToList())
                .Create();
        }

        [Given(@"the Employer Incentives Api receives the recalculate earnings request")]
        public void GivenTheEmployerIncentivesApiReceivesTheRecalculateEarningsRequest()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/earningsRecalculations")
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.NoContent)
                );
        }

        [When(@"the Outer Api receives the recalculate earnings request")]
        public async Task WhenTheOuterApiReceivesTheRecalculateEarningsRequest()
        {
            _response = await _context.OuterApiClient.PostAsync($"/earningsRecalculations", new StringContent(JsonSerializer.Serialize(_request), Encoding.UTF8, "application/json"));
        }

        [Then(@"the respsonde code of NoContent is returned")]
        public void ThenTheRespsondeCodeOfNoContentIsReturned()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

    }
}

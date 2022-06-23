using FluentAssertions;
using SFA.DAS.EmployerIncentives.Api.Models;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "EmploymentCheck")]
    public class EmploymentCheckSteps
    {
        private readonly TestContext _context;
        private HttpResponseMessage _response;
        private UpdateEmploymentCheckRequest _request;

        public EmploymentCheckSteps(TestContext testContext)
        {
            _context = testContext;
        }

        [Given(@"the caller wants to update an employment check")]
        public void GivenTheCallerWantsToUpdateAnEmploymentCheck()
        {
            _request = new UpdateEmploymentCheckRequest
            {
                CorrelationId = System.Guid.NewGuid(),
                Result = "Found",
                DateChecked = System.DateTime.Now.AddMinutes(-10)
            };
        }

        [Given(@"the Employer Incentives Api receives the employment check update request")]
        public void GivenTheEmployerIncentivesApiReceivesTheEmploymentCheckRequest()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/employmentchecks/{_request.CorrelationId}")
                        .UsingPut())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                );
        }

        [When(@"the Outer Api receives the employment check update request")]
        public async Task WhenTheOuterApiReceivesTheEmploymentCheckRequest()
        {
            _response = await _context.OuterApiClient.PutAsync($"/employmentchecks/{_request.CorrelationId}", new StringContent(JsonSerializer.Serialize(_request), Encoding.UTF8, "application/json"));
        }

        [Then(@"the response code of Ok is returned")]
        public void ThenTheResponseCodeOfOkIsReturned()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

    }
}

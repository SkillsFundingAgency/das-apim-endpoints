using System;
using FluentAssertions;
using SFA.DAS.EmployerIncentives.Api.Models;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Application.Commands.RegisterEmploymentCheck;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "RegisterEmploymentCheck")]
    public class RegisterEmploymentCheckSteps
    {
        private readonly TestContext _context;
        private HttpResponseMessage _response;
        private RegisterCheckRequest _request;

        public RegisterEmploymentCheckSteps(TestContext testContext)
        {
            _context = testContext;
        }

        [Given(@"the caller wants to register an employment check")]
        public void GivenTheCallerWantsToRegisterAnEmploymentCheck()
        {
            _request = new RegisterCheckRequest
            {
                CorrelationId = System.Guid.NewGuid(),
                ApprenticeshipAccountId = 123,
                ApprenticeshipId = 456,
                CheckType = "1stCheck",
                MaxDate = DateTime.Now,
                MinDate = DateTime.Now.AddDays(-7),
                Uln = 1234356
            };
        }

        [Given(@"the Employment Check Api receives the register employment check request")]
        public void GivenTheEmploymentCheckApiReceivesTheRegisterEmploymentCheckRequest()
        {
            _context.EmploymentCheckApi.MockServer
                .Given(
                    Request.Create().WithPath($"/api/EmploymentCheck/RegisterCheck")
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithBody(JsonConvert.SerializeObject(new RegisterEmploymentCheckResponse()))
                ); 
        }

        [When(@"the Outer Api receives the register employment check request")]
        public async Task WhenTheOuterApiReceivesTheRegisterEmploymentCheckRequest()
        {
            _response = await _context.OuterApiClient.PostAsync($"/employmentchecks", new StringContent(JsonSerializer.Serialize(_request), Encoding.UTF8, "application/json"));
        }

        [Then(@"the response code of Ok is returned")]
        public void ThenTheResponseCodeOfOkIsReturned()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

    }
}

using AutoFixture;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmploymentCheck.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "RegisterCheckSuccess")]
    public class RegisterCheckSuccessSteps
    {
        private readonly Fixture _fixture;
        private readonly TestContext _context;
        private HttpResponseMessage? _response;

        public RegisterCheckSuccessSteps(TestContext context)
        {
            _fixture = new Fixture();
            _context = context;
        }

        [Given(@"an employer has applied for Apprenticeship Incentive for an apprentice")]
        public void GivenAnEmployerHasAppliedForApprenticeshipIncentiveForAnApprentice()
        {
            // blank
        }
        
        [When(@"the Employer Incentives service are validating employment of the apprentice")]
        public async Task WhenTheEmployerIncentivesServiceAreValidatingEmploymentOfTheApprentice()
        {
            dynamic check = new
            {
                CorrelationId = Guid.NewGuid(),
                CheckType = _fixture.Create<string>(),
                Uln = _fixture.Create<long>(),
                ApprenticeshipAccountId = _fixture.Create<long>(),
                ApprenticeshipId = _fixture.Create<long>(),
                MinDate = DateTime.Now.AddDays(-100),
                MaxDate = DateTime.Now.AddDays(-90)
            };

            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath("/EmploymentCheck/RegisterCheck")
                        .UsingPost()
                    )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK));

            _response = await _context.OuterApiClient.PostAsync("api/EmploymentCheck/RegisterCheck", new StringContent(JsonSerializer.Serialize(check), Encoding.UTF8, "application/json"));
        }

        [Then(@"a new Employment Check request is registered in Employment Check system")]
        public void ThenANewEmploymentCheckRequestIsRegisteredInEmploymentCheckSystem()
        {
            _response?.EnsureSuccessStatusCode();
        }

    }
}

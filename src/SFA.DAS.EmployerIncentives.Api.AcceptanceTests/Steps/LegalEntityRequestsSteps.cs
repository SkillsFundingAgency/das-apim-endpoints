using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using SFA.DAS.EmployerIncentives.Models.PassThrough;
using TechTalk.SpecFlow;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "LegalEntityRequests")]
    public class LegalEntityRequestsSteps
    {
        private readonly TestContext _context;
        private long _accountId;
        private long _accountLegalEntityId;
        private LegalEntityRequest _request;
        private TestResult _innerResult;
        private HttpResponseMessage _response;
        private HttpStatusCode _innerResponseStatusCode;
        private readonly Fixture _fixture;

        public LegalEntityRequestsSteps(TestContext context)
        {
            _fixture = new Fixture();
            _context = context;
        }

        [Given(@"the caller wants to add a LegalEntity to an Account")]
        public void GivenTheCallerWantsToAddALegalEntityToAnAccount()
        {
            _accountId = _fixture.Create<long>();
            _request = _fixture.Create<LegalEntityRequest>();
        }

        [Given(@"the Employer Incentives Api receives the add request")]
        public void GivenTheEmployerIncentivesApiShouldReceiveTheRequestToAddThisLegalEntityToAnAccount()
        {
            _innerResult = _fixture.Create<TestResult>();
            _innerResponseStatusCode = HttpStatusCode.Created;

            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/accounts/{_accountId}/legalentities")
                        .WithBody(new JsonMatcher(JsonSerializer.Serialize(_request), true))
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)_innerResponseStatusCode)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonSerializer.Serialize(_innerResult))
                );
        }

        [Given(@"the Employer Incentives Api will error when receiving the add request")]
        public void GivenTheEmployerIncentivesApiWillErrorWhenReceivingTheAddRequest()
        {
            _innerResponseStatusCode = HttpStatusCode.InternalServerError;

            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/accounts/{_accountId}/legalentities")
                        .WithBody(new JsonMatcher(JsonSerializer.Serialize(_request), true))
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)_innerResponseStatusCode)
                        .WithHeader("Content-Type", "plain/text")
                        .WithBody("Error occurred")
                );
        }

        [Given(@"the Employer Incentives Api receives the remove request")]
        public void GivenTheEmployerIncentivesApiReceivesTheRemoveRequest()
        {
            _innerResponseStatusCode = HttpStatusCode.NoContent;

            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/accounts/{_accountId}/legalentities/{_accountLegalEntityId}")
                        .UsingDelete())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)_innerResponseStatusCode)
                );
        }

        [Given(@"the caller wants to remove a LegalEntity from an Account")]
        public void GivenTheCallerWantsToRemoveALegalEntityFromAnAccount()
        {
            _accountId = _fixture.Create<long>();
            _accountLegalEntityId = _fixture.Create<long>();
        }

        [Given(@"the caller wants to get all LegalEntities for an Account")]
        public void GivenTheCallerWantsToGetAllLegalEntitiesForAnAccount()
        {
            _accountId = _fixture.Create<long>();
        }


        [Given(@"the Employer Incentives Api receives the query request")]
        public void GivenTheEmployerIncentivesApiReceivesTheQueryRequest()
        {
            _innerResult = _fixture.Create<TestResult>();
            _innerResponseStatusCode = HttpStatusCode.OK;

            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/accounts/{_accountId}/legalentities")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)_innerResponseStatusCode)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonSerializer.Serialize(_innerResult))
                );
        }

        [When(@"the Outer Api receives the add request")]
        public async Task WhenTheOuterApiReceivesTheRequestToAddALegalEntityToAnAccount()
        {
           _response = await  _context.OuterApiClient.PostAsJsonAsync($"accounts/{_accountId}/legalentities", _request);
        }

        [When(@"the Outer Api receives the remove request")]
        public async Task WhenTheOuterApiReceivesTheRemoveRequest()
        {
            _response = await _context.OuterApiClient.DeleteAsync($"accounts/{_accountId}/legalentities/{_accountLegalEntityId}");
        }

        [When(@"the Outer Api receives the query request")]
        public async Task WhenTheOuterApiReceivesTheQueryRequest()
        {
            _response = await _context.OuterApiClient.GetAsync($"accounts/{_accountId}/legalentities");
        }

        [Then(@"the response from the Employer Incentives Inner Api is returned")]
        public async Task ThenReturnTheResultFromEmployerIncentivesApiToTheCaller()
        {
            _response.StatusCode.Should().Be(_innerResponseStatusCode);
            var json = JsonSerializer.Deserialize<TestResult>(await _response.Content.ReadAsStringAsync(), null);
            json.Should().BeEquivalentTo(_innerResult);
        }

        [Then(@"the response from the Employer Incentives Inner Api has no content")]
        public async Task ThenTheResponseFromTheEmployerIncentivesInnerApiHasNoContent()
        {
            _response.StatusCode.Should().Be(_innerResponseStatusCode);
            var body = await _response.Content.ReadAsStringAsync();
            body.Should().BeNullOrWhiteSpace();
        }
    }

    public class TestResult
    {
        public string SomeField { get; set; }
        public string SomeOtherField { get; set; }
        public string YetAnotherField { get; set; }
    }
}

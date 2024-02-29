using AutoFixture;
using FluentAssertions;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Api.Models;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using System.Collections.Generic;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "PausePayments")]
    public class PausePaymentsSteps
    {
        private readonly TestContext _context;
        private PostPausePaymentsRequest _request;
        private HttpResponseMessage _response;
        private HttpStatusCode _innerResponseStatusCode;
        private SingleMessageResponse _innerResponse;
        private readonly Fixture _fixture;

        public PausePaymentsSteps(TestContext context)
        {
            _fixture = new Fixture();
            _context = context;
        }

        [Given(@"the caller wants to pause payments for an apprenticeship incentive")]
        public void GivenTheCallerWantsToPauseResumePayments()
        {
            _request = new PostPausePaymentsRequest(
                new PausePaymentsRequest
                {
                    Action = PausePaymentsAction.Pause,
                    Applications = new List<InnerApi.Requests.Application>()
                    {
                        _fixture.Create<InnerApi.Requests.Application>(),
                        new InnerApi.Requests.Application(){
                            AccountLegalEntityId = _fixture.Create<long>(),
                            ULN = _fixture.Create<long>(),
                        },
                        _fixture.Create<InnerApi.Requests.Application>()
                    }.ToArray(),
                    ServiceRequest = _fixture.Create<ServiceRequest>()
                });
        }

        [Given(@"the Employer Incentives Api receives request")]
        public void GivenTheEmployerIncentivesApiShouldReceiveTheRequest()
        {
            _innerResponseStatusCode = HttpStatusCode.OK;

            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/pause-payments")
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)_innerResponseStatusCode)                        
                );
        }


        [Given(@"the Employer Incentives Api receives request but cannot find the incentive")]
        public void GivenTheEmployerIncentivesApiReceivesRequestButCannotFindTheIncentive()
        {

            _innerResponseStatusCode = HttpStatusCode.NotFound;
            _innerResponse = new SingleMessageResponse { Message = "Not Found ...."}; 

            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/pause-payments")
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)_innerResponseStatusCode)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonConvert.SerializeObject(_innerResponse))
                );
        }

        [Given(@"the Employer Incentives Api receives request but returns an already paused status")]
        public void GivenTheEmployerIncentivesApiReceivesRequestButReturnsAnAlreadyPausedStatus()
        {
            _innerResponseStatusCode = HttpStatusCode.BadRequest;
            _innerResponse = new SingleMessageResponse { Message = "Already paused ...." };

            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/pause-payments")
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)_innerResponseStatusCode)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonConvert.SerializeObject(_innerResponse))
                );
        }


        [When(@"the Outer Api receives the Pause Payments request")]
        public async Task WhenTheOuterApiReceivesThePausePaymentsRequest()
        {
           _response = await  _context.OuterApiClient.PostAsJsonAsync($"pause-payments", _request);
        }

        [Then(@"the response of OK is returned")]
        public void ThenReturnOkToTheCaller()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Then(@"the response of Not Found is returned")]
        public void ThenReturnNotFoundToTheCaller()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Then(@"the response of Bad Request is returned")]
        public void ThenReturnBadRequestToTheCaller()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Then(@"the response body contains the full message")]
        public async Task ThenReturnNotFoundBodyToTheCaller()
        {
            var content = await _response.Content.ReadAsStringAsync();

            var errorMessage = JsonConvert.DeserializeObject<SingleMessageResponse>(content);
            errorMessage.Message.Should().Be(_innerResponse.Message);
        }

    }
}

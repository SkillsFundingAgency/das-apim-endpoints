using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "RevertPayments")]
    public class RevertPaymentsSteps
    {
        private readonly TestContext _context;
        private PostRevertPaymentsRequest _request;
        private HttpResponseMessage _response;
        private readonly Fixture _fixture;

        public RevertPaymentsSteps(TestContext context)
        {
            _fixture = new Fixture();
            _context = context;
        }

        [Given(@"the caller wants to revert payments for an apprenticeship incentive")]
        public void GivenTheCallerWantsToRevertPayments()
        {
            _request = new PostRevertPaymentsRequest (
                new RevertPaymentsRequest
                {
                    ServiceRequest = _fixture.Create<ServiceRequest>(),
                    Payments = _fixture.CreateMany<Guid>(10).ToList()
                });
        }

        [Given(@"the Employer Incentives Api receives the request")]
        public void GivenTheEmployerIncentivesApiReceivesTheRequest()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/revert-payments")
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                );
        }

        [Given(@"the Employer Incentives Api receives request but returns a payment not found status")]
        public void GivenTheEmployerIncentivesApiReceivesTheRequestButPaymentNotFound()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/revert-payments")
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.BadRequest)
                        .WithBody("Payment not found")
                );
        }

        [When(@"the Outer Api receives the Revert Payments request")]
        public async Task WhenTheOuterApiReceivesTheRevertPaymentsRequest()
        {
            _response = await _context.OuterApiClient.PostAsJsonAsync($"revert-payments", _request);
        }

        [Then(@"the response of OK is returned")]
        public void ThenReturnOkToTheCaller()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.OK);
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

            content.Should().Be("Payment not found");
        }
    }
}

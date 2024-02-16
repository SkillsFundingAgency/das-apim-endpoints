using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
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
    [Scope(Feature = "ReinstatePayments")]
    public class ReinstatePaymentsSteps
    {
        private readonly TestContext _context;
        private ReinstatePaymentsRequest _request;
        private HttpResponseMessage _response;
        private readonly Fixture _fixture;

        public ReinstatePaymentsSteps(TestContext context)
        {
            _fixture = new Fixture();
            _context = context;
        }

        [Given(@"the caller wants to reinstate payments for apprenticeship incentives")]
        public void GivenTheCallerWantsToReinstatePayments()
        {
            _request = new ReinstatePaymentsRequest
            {
                ServiceRequest = _fixture.Create<ReinstatePaymentsServiceRequest>(),
                Payments = _fixture.CreateMany<Guid>(10).ToList()
            };
        }

        [Given(@"the Employer Incentives Api receives the request")]
        public void GivenTheEmployerIncentivesApiReceivesTheRequest()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/reinstate-payments")
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                );
        }

        [Given(@"the Employer Incentives Api receives the request but returns a payment not found status")]
        public void GivenTheEmployerIncentivesApiReceivesTheRequestButPaymentNotFound()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/reinstate-payments")
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.BadRequest)
                        .WithBody("Payment not found")
                );
        }

        [When(@"the Outer Api receives the Reinstate Payments request")]
        public async Task WhenTheOuterApiReceivesTheReinstatePaymentsRequest()
        {
            _response = await _context.OuterApiClient.PostAsJsonAsync($"reinstate-payments", _request);
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

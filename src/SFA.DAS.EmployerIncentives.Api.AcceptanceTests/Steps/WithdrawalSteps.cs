using AutoFixture;
using FluentAssertions;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "Withdrawal")]
    public class WithdrawalSteps
    {
        private readonly TestContext _context;
        private PostWithdrawApplicationRequest _withdrawRequest;
        private PostReinstateApplicationRequest _reinstateRequest;
        private HttpResponseMessage _response;
        private HttpStatusCode _innerResponseStatusCode;
        private readonly Fixture _fixture;

        public WithdrawalSteps(TestContext context)
        {
            _fixture = new Fixture();
            _context = context;
        }

        [Given(@"the caller wants to Withdraw an apprenticeship application")]
        public void GivenTheCallerWantsToWithdrawAnApprenticeshipApplication()
        {
            _withdrawRequest = new PostWithdrawApplicationRequest(
                new WithdrawRequest
                {
                    WithdrawalType = WithdrawalType.Employer,
                    AccountLegalEntityId = _fixture.Create<long>(),
                    ULN = _fixture.Create<long>(),
                    ServiceRequest = _fixture.Create<ServiceRequest>()
                });
        }

        [Given(@"the caller wants to Reinstate an apprenticeship application")]
        public void GivenTheCallerWantsToReinstateAnApprenticeshipApplication()
        {
            _reinstateRequest = new PostReinstateApplicationRequest(
                new ReinstateApplicationRequest
                {
                    Applications = new[] { new InnerApi.Requests.Application { AccountLegalEntityId = _fixture.Create<long>(), ULN = _fixture.Create<long>() } }
                });
        }

        [Given(@"the Employer Incentives Api receives the Withdrawal request")]
        public void GivenTheEmployerIncentivesApiShouldReceiveTheWithdrawalRequest()
        {
            _innerResponseStatusCode = HttpStatusCode.Accepted;

            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/withdrawals")
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)_innerResponseStatusCode)                        
                );
        }

        [Given(@"the Employer Incentives Api receives the Reinstate request")]
        public void GivenTheEmployerIncentivesApiShouldReceiveTheReinstateRequest()
        {
            _innerResponseStatusCode = HttpStatusCode.Accepted;

            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/withdrawal-reinstatements")
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)_innerResponseStatusCode)
                );
        }

        [When(@"the Outer Api receives the Withdrawal request")]
        public async Task WhenTheOuterApiReceivesTheWithdrawalRequest()
        {
           _response = await _context.OuterApiClient.PostAsJsonAsync($"withdrawals", _withdrawRequest);
        }

        [When(@"the Outer Api receives the Reinstate request")]
        public async Task WhenTheOuterApiReceivesTheReinstateRequest()
        {
            _response = await _context.OuterApiClient.PostAsJsonAsync($"withdrawal-reinstatements", _reinstateRequest);
        }

        [Then(@"the response of Accepted is returned")]
        public void ThenReturnAcceptedToTheCaller()
        {
            _response.StatusCode.Should().Be((int)HttpStatusCode.Accepted);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorBlock;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "BlockAccountLegalEntitiesForPayment")]
    public class BlockAccountLegalEntitiesForPaymentsSteps
    {
        private readonly TestContext _context;
        private readonly Fixture _fixture;
        private List<BlockAccountLegalEntityForPaymentsRequest> _request;
        private HttpResponseMessage _response;

        public BlockAccountLegalEntitiesForPaymentsSteps(TestContext testContext)
        {
            _context = testContext;
            _fixture = new Fixture();
        }

        [Given(@"the caller wants to block account legal entities for payment")]
        public void GivenTheCallerWantsToBlockAccountLegalEntitiesForPayment()
        {
            var request = _fixture.Build<BlockAccountLegalEntityForPaymentsRequest>()
                .With(x => x.ServiceRequest, _fixture.Create<ServiceRequest>())
                .With(x => x.VendorBlocks, new List<VendorBlock>
                {
                    _fixture.Build<VendorBlock>()
                        .With(x => x.VendorBlockEndDate, DateTime.Today.AddMonths(1))
                        .Create(),
                    _fixture.Build<VendorBlock>()
                        .With(x => x.VendorBlockEndDate, DateTime.Today.AddMonths(2))
                        .Create()
                })
                .Create();

            _request = new List<BlockAccountLegalEntityForPaymentsRequest> { request };
        }

        [Given(@"the Employer Incentives Api receives the block payments request")]
        public void GivenTheEmployerIncentivesApiReceivesTheBlockPaymentsRequest()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath("/blockedpayments")
                        .UsingPatch())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.NoContent)
                );
        }

        [When(@"the Outer Api receives the block payments request")]
        public async Task WhenTheOuterApiReceivesTheBlockPaymentsRequest()
        {
            _response = await _context.OuterApiClient.PatchAsync("/blockedpayments",
                new StringContent(JsonSerializer.Serialize(_request), Encoding.UTF8, "application/json"));
        }

        [Then(@"the response code of NoContent is returned")]
        public void ThenTheResponseCodeOfNoContentIsReturned()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
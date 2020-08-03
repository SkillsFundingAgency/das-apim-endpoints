using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.Models.Commitments;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "GettingEligibleApprentices")]
    public class GettingEligibleApprenticesSteps
    {
        private readonly TestContext _context;
        private long _accountId;
        private long _accountLegalEntityId;
        private Fixture _fixture;
        private HttpResponseMessage _response;
        private ApprenticeshipItem _eligibleApprenticeship1;
        private ApprenticeshipItem _eligibleApprenticeship2;
        private ApprenticeshipItem _nonEligibleApprenticeship3;
        private ApprenticeshipItem _nonEligibleApprenticeship4;
        private ApprenticeshipItem _nonEligibleApprenticeship5;

        public GettingEligibleApprenticesSteps(TestContext testContext)
        {
            _fixture = new Fixture();
            _context = testContext;
        }

        [Given(@"the caller wants to search for eligible apprentices by Account Id and AccountLegalEntity Id")]
        public void GivenTheCallerWantsToSearchForEligibleApprenticesForAccountIdAndAccountLegalEntityId()
        {
            _accountId = _fixture.Create<long>();
            _accountLegalEntityId = _fixture.Create<long>();
        }
        
        [Given(@"this search request finds no approved apprenticeships")]
        public void GivenThisSearchRequestFindsNoApprovedApprenticeships()
        {
            var response = new ApprenticeshipSearchResponse
            {
                Apprenticeships = new ApprenticeshipItem[0]
            };

            SetApprenticeshipSearchToReturn(response);
        }

        [Given(@"this search request finds several approved apprenticeships")]
        public void GivenThisSearchRequestFindsSeveralApprovedApprenticeships()
        {
            _eligibleApprenticeship1 = _fixture.Create<ApprenticeshipItem>();
            _eligibleApprenticeship2 = _fixture.Create<ApprenticeshipItem>();
            _nonEligibleApprenticeship3 = _fixture.Create<ApprenticeshipItem>();
            _nonEligibleApprenticeship4 = _fixture.Create<ApprenticeshipItem>();
            _nonEligibleApprenticeship5 = _fixture.Create<ApprenticeshipItem>();

            var response = new ApprenticeshipSearchResponse
            {
                Apprenticeships = new ApprenticeshipItem[]
                {
                    _eligibleApprenticeship1, _eligibleApprenticeship2, _nonEligibleApprenticeship3,
                    _nonEligibleApprenticeship4, _nonEligibleApprenticeship5
                }
            };

            SetApprenticeshipSearchToReturn(response);
        }

        [Given(@"two of these are eligible")]
        public void GivenTwoOfTheseAreEligible()
        {
            SetEligibleApprenticeshipToReturnHttpStatusCode(HttpStatusCode.OK, _eligibleApprenticeship1.Uln);
            SetEligibleApprenticeshipToReturnHttpStatusCode(HttpStatusCode.OK, _eligibleApprenticeship2.Uln);
        }

        [When(@"the Outer Api receives the request to list all eligible apprentices")]
        public async Task WhenTheOuterApiReceivesTheRequestToListAllEligibleApprentices()
        {
            _response = await _context.OuterApiClient.GetAsync($"/apprentices?accountId={_accountId}&accountLegalEntityId={_accountLegalEntityId}");
        }
        
        [Then(@"the result should return Ok, but with no apprentices")]
        public async Task ThenTheResultShouldReturnOkButWithNoApprentices()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await _response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<EligibleApprenticeshipsResponse>(content);
            result.Should().NotBeNull();
            result.Apprentices.Should().BeNull();
        }

        [Then(@"the result should return Ok and have two apprentices")]
        public async Task ThenTheResultShouldReturnOkAndHaveTwoApprentices()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await _response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<EligibleApprenticeshipsResponse>(content, new JsonSerializerOptions{ PropertyNameCaseInsensitive = true });
            result.Should().NotBeNull();
            result.Apprentices.Length.Should().Be(2);
            result.Apprentices.Count(a => a.Uln == _eligibleApprenticeship1.Uln).Should().Be(1);
            result.Apprentices.Count(a => a.Uln == _eligibleApprenticeship2.Uln).Should().Be(1);
        }

        private void SetApprenticeshipSearchToReturn(ApprenticeshipSearchResponse response)
        {
            _context.CommitmentsV2InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/api/apprenticeships")
                        .WithParam("accountId", _accountId.ToString())
                        .WithParam("accountLegalEntityId", _accountLegalEntityId.ToString())
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonSerializer.Serialize(response))
                );
        }

        private void SetEligibleApprenticeshipToReturnHttpStatusCode(HttpStatusCode statusCode, long uln)
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/eligible-apprenticeships/{uln}")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)statusCode)
                );
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Commitments;
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
        private DateTime _eligibilityStartDate;
        private DateTime _eligibilityEndDate;

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
            SetupIncentiveDetailsResponse();

            var response = new ApprenticeshipSearchResponse
            {
                Apprenticeships = new ApprenticeshipItem[0]
            };

            SetApprenticeshipSearchToReturn(response);
        }

        [Given(@"this search request finds several approved apprenticeships")]
        public void GivenThisSearchRequestFindsSeveralApprovedApprenticeships()
        {
            SetupIncentiveDetailsResponse();

            _eligibleApprenticeship1 = _fixture.Build<ApprenticeshipItem>().With(a => a.ApprenticeshipStatus, ApprenticeshipStatus.Live).Create();
            _eligibleApprenticeship2 = _fixture.Build<ApprenticeshipItem>().With(a => a.ApprenticeshipStatus, ApprenticeshipStatus.Live).Create();
            _nonEligibleApprenticeship3 = _fixture.Build<ApprenticeshipItem>().With(a => a.ApprenticeshipStatus, ApprenticeshipStatus.Live).Create();
            _nonEligibleApprenticeship4 = _fixture.Build<ApprenticeshipItem>().With(a => a.ApprenticeshipStatus, ApprenticeshipStatus.Live).Create();
            _nonEligibleApprenticeship5 = _fixture.Build<ApprenticeshipItem>().With(a => a.ApprenticeshipStatus, ApprenticeshipStatus.Live).Create();

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

        [Given(@"this search request finds several approved apprenticeships which are stopped")]
        public void GivenThisSearchRequestFindsSeveralApprovedApprenticeshipsWhichAreStopped()
        {
            SetupIncentiveDetailsResponse();

            _eligibleApprenticeship1 = _fixture.Build<ApprenticeshipItem>().With(a => a.ApprenticeshipStatus, ApprenticeshipStatus.Stopped).Create();
            _eligibleApprenticeship2 = _fixture.Build<ApprenticeshipItem>().With(a => a.ApprenticeshipStatus, ApprenticeshipStatus.Stopped).Create();
            _nonEligibleApprenticeship3 = _fixture.Build<ApprenticeshipItem>().With(a => a.ApprenticeshipStatus, ApprenticeshipStatus.Stopped).Create();
            _nonEligibleApprenticeship4 = _fixture.Build<ApprenticeshipItem>().With(a => a.ApprenticeshipStatus, ApprenticeshipStatus.Stopped).Create();
            _nonEligibleApprenticeship5 = _fixture.Build<ApprenticeshipItem>().With(a => a.ApprenticeshipStatus, ApprenticeshipStatus.Stopped).Create();

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
            _response = await _context.OuterApiClient.GetAsync($"/apprenticeships?accountId={_accountId}&accountLegalEntityId={_accountLegalEntityId}");
        }
        
        [Then(@"the result should return Ok, but with no apprentices")]
        public async Task ThenTheResultShouldReturnOkButWithNoApprentices()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await _response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<IEnumerable<EligibleApprenticeshipDto>>(content);
            result.Should().NotBeNull();
        }

        [Then(@"the result should return Ok and have two apprentices")]
        public async Task ThenTheResultShouldReturnOkAndHaveTwoApprentices()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await _response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<IEnumerable<EligibleApprenticeshipDto>>(content, new JsonSerializerOptions{ PropertyNameCaseInsensitive = true });
            result.Should().NotBeNull();
            result.Count().Should().Be(2);
            result.Count(a => a.Uln == _eligibleApprenticeship1.Uln).Should().Be(1);
            result.Count(a => a.Uln == _eligibleApprenticeship2.Uln).Should().Be(1);
        }

        private void SetupIncentiveDetailsResponse()
        {
            _eligibilityStartDate = _fixture.Create<DateTime>();
            _eligibilityEndDate = _fixture.Create<DateTime>();
            var response = new GetIncentiveDetailsResponse {  EligibilityStartDate = _eligibilityStartDate, EligibilityEndDate = _eligibilityEndDate };
            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/newapprenticeincentive")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonSerializer.Serialize(response)));
        }

        private void SetApprenticeshipSearchToReturn(ApprenticeshipSearchResponse response)
        {
            _context.CommitmentsV2InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/api/apprenticeships")
                        .WithParam("accountId", _accountId.ToString())
                        .WithParam("accountLegalEntityId", _accountLegalEntityId.ToString())
                        .WithParam("startDateRangeFrom", _eligibilityStartDate.ToString("u"))
                        .WithParam("startDateRangeTo", _eligibilityEndDate.ToString("u"))
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

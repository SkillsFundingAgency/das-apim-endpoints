using AutoFixture;
using FluentAssertions;
using SFA.DAS.EmployerIncentives.Application.Queries.GetApplications;
using SFA.DAS.EmployerIncentives.InnerApi;
using SFA.DAS.EmployerIncentives.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "GetApplicationsForAccount")]
    public class GetApplicationsForAccountSteps 
    {
        private readonly TestContext _context;
        private long _accountId;
        private long _accountLegalEntityId;
        private Fixture _fixture;
        private HttpResponseMessage _response;

        public GetApplicationsForAccountSteps(TestContext testContext)
        {
            _fixture = new Fixture();
            _context = testContext;
        }

        [Given(@"the caller wants to search for apprentice applications by Account Id and Account Legal Entity Id")]
        public void GivenTheCallerWantsToSearchForApprenticeApplicationsByAccountId()
        {
            _accountId = _fixture.Create<long>();
            _accountLegalEntityId = _fixture.Create<long>();
        }

        [Given(@"this search request finds no applications")]
        public void GivenThisSearchRequestFindsNoApplications()
        {
            var applications = new List<ApprenticeApplication>();
            var response = new GetApplicationsResult { ApprenticeApplications = applications, BankDetailsStatus = InnerApi.BankDetailsStatus.NotSupplied, FirstSubmittedApplicationId = Guid.NewGuid() };
            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/accounts/{_accountId}/legalentity/{_accountLegalEntityId}/applications")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonSerializer.Serialize(response)));
        }

        [When(@"the Outer Api receives the request to list all applications")]
        public async Task WhenTheOuterApiReceivesTheRequestToListAllApplications()
        {
            _response = await _context.OuterApiClient.GetAsync($"/accounts/{_accountId}/legalentity/{_accountLegalEntityId}/applications");
        }

        [Then(@"the result should return Ok, but with no applications")]
        public async Task ThenTheResultShouldReturnOkButWithNoApplications()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await _response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var result = JsonSerializer.Deserialize<GetApplicationsResult>(content, options);
            result.Should().NotBeNull();
            result.ApprenticeApplications.Count().Should().Be(0);
            result.BankDetailsStatus.Should().Be(BankDetailsStatus.NotSupplied);
        }

        [Given(@"this search request finds one submitted application")]
        public void GivenThisSearchRequestFindsOneSubmittedApplication()
        {
            var submittedApplication = _fixture.Build<ApprenticeApplication>()
                .With(x => x.FirstPaymentStatus, _fixture.Build<PaymentStatus>()
                    .Without(x => x.EmploymentCheckPassed).Create())
                .Create();
            var applications = new List<ApprenticeApplication> { submittedApplication };
            var response = new GetApplicationsResult { ApprenticeApplications = applications, BankDetailsStatus = InnerApi.BankDetailsStatus.InProgress, FirstSubmittedApplicationId = Guid.NewGuid() };

            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/accounts/{_accountId}/legalentity/{_accountLegalEntityId}/applications")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonSerializer.Serialize(response)));
        }

        [Then(@"the result should return Ok, with one submitted application")]
        public async Task ThenTheResultShouldReturnOkWithOneSubmittedApplication()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await _response.Content.ReadAsStringAsync();
            
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var result = JsonSerializer.Deserialize<GetApplicationsResult>(content, options);
            result.Should().NotBeNull();
            result.ApprenticeApplications.Count().Should().Be(1);
            result.BankDetailsStatus.Should().Be(BankDetailsStatus.InProgress);
        }

        [Given(@"this search request finds one application for multiple apprentices")]
        public void GivenThisSearchRequestFindsOneApplicationForMultipleApprentices()
        {
            var multipleApprenticesApplication = _fixture.CreateMany<ApprenticeApplication>(10);

            var response = new GetApplicationsResult { ApprenticeApplications = multipleApprenticesApplication, BankDetailsStatus = InnerApi.BankDetailsStatus.Completed, FirstSubmittedApplicationId = Guid.NewGuid() };

            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/accounts/{_accountId}/legalentity/{_accountLegalEntityId}/applications")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonSerializer.Serialize(response)));
        }

        [Then(@"the result should return Ok, with one submitted application for multiple apprentices")]
        public async Task ThenTheResultShouldReturnOkWithOneSubmittedApplicationForMultipleApprentices()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await _response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var result = JsonSerializer.Deserialize<GetApplicationsResult>(content, options);
            result.ApprenticeApplications.Should().NotBeNull();
            result.ApprenticeApplications.Count().Should().Be(10);
            result.BankDetailsStatus.Should().Be(BankDetailsStatus.Completed);
        }

        [Given(@"this search request finds multiple submitted applications")]
        public void GivenThisSearchRequestFindsMultipleSubmittedApplications()
        {
            var multipleApplications = _fixture.CreateMany<ApprenticeApplication>(2).ToList();
            var response = new GetApplicationsResult { ApprenticeApplications = multipleApplications, BankDetailsStatus = InnerApi.BankDetailsStatus.Rejected, FirstSubmittedApplicationId = Guid.NewGuid() };

            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/accounts/{_accountId}/legalentity/{_accountLegalEntityId}/applications")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonSerializer.Serialize(response)));
        }

        [Then(@"the result should return Ok, with multiple submitted applications")]
        public async Task ThenTheResultShouldReturnOkWithMultipleSubmittedApplications()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await _response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var result = JsonSerializer.Deserialize<GetApplicationsResult>(content, options);
            result.Should().NotBeNull();
            result.ApprenticeApplications.Count().Should().Be(2);
            result.BankDetailsStatus.Should().Be(BankDetailsStatus.Rejected);
        }
    }
}

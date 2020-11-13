using AutoFixture;
using FluentAssertions;
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
            var response = new List<ApprenticeApplication>();
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
            var result = JsonSerializer.Deserialize<IEnumerable<ApprenticeApplication>>(content, options);
            result.Should().NotBeNull();
            result.Count().Should().Be(0);
        }

        [Given(@"this search request finds one in progress application")]
        public void GivenThisSearchRequestFindsOneInProgressApplication()
        {
            var inProgressApplication = _fixture.Create<ApprenticeApplication>();
            inProgressApplication.Status = "InProgress";
            var response = new List<ApprenticeApplication>
            {
                inProgressApplication
            };
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

        [Then(@"the result should return Ok, with one in progress application")]
        public async Task ThenTheResultShouldReturnOkWithOneInProgressApplication()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await _response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var result = JsonSerializer.Deserialize<IEnumerable<ApprenticeApplication>>(content, options);
            result.Should().NotBeNull();
            result.Count().Should().Be(1);
            result.ToList()[0].Status.Should().Be("InProgress");
        }

        [Given(@"this search request finds one submitted application")]
        public void GivenThisSearchRequestFindsOneSubmittedApplication()
        {
            var submittedApplication = _fixture.Create<ApprenticeApplication>();
            submittedApplication.Status = "Submitted";
            var response = new List<ApprenticeApplication> { submittedApplication };

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
            var result = JsonSerializer.Deserialize<List<ApprenticeApplication>>(content, options);
            result.Should().NotBeNull();
            result.Count().Should().Be(1);
            result.ToList()[0].Status.Should().Be("Submitted");
        }

        [Given(@"this search request finds one application for multiple apprentices")]
        public void GivenThisSearchRequestFindsOneApplicationForMultipleApprentices()
        {
            var multipleApprenticesApplication = _fixture.CreateMany<ApprenticeApplication>(10);
            var applicationId = _fixture.Create<Guid>();
            foreach(var application in multipleApprenticesApplication)
            {
                application.Status = "Submitted";
                application.ApplicationId = applicationId;
            }

            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/accounts/{_accountId}/legalentity/{_accountLegalEntityId}/applications")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonSerializer.Serialize(multipleApprenticesApplication)));
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
            var result = JsonSerializer.Deserialize<IEnumerable<ApprenticeApplication>>(content, options);
            result.Should().NotBeNull();
            result.Count().Should().Be(10);
        }

        [Given(@"this search request finds multiple submitted applications")]
        public void GivenThisSearchRequestFindsMultipleSubmittedApplications()
        {
            var multipleApplications = _fixture.CreateMany<ApprenticeApplication>(2).ToList();
            multipleApplications[0].Status = "Submitted";
            multipleApplications[1].Status = "Submitted";

            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/accounts/{_accountId}/legalentity/{_accountLegalEntityId}/applications")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonSerializer.Serialize(multipleApplications)));
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
            var result = JsonSerializer.Deserialize<IEnumerable<ApprenticeApplication>>(content, options);
            result.Should().NotBeNull();
            result.Count().Should().Be(2);
            var applications = result.ToList();
            applications[0].Status.Should().Be("Submitted");
            applications[1].Status.Should().Be("Submitted");
            applications[0].ApplicationId.Should().NotBe(applications[1].ApplicationId);
        }

        [Given(@"this search request finds multiple applications with statuses of in progress and submitted")]
        public void GivenThisSearchRequestFindsMultipleApplicationsWithStatusesOfInProgressAndSubmitted()
        {
            var multipleApplications = _fixture.CreateMany<ApprenticeApplication>(2).ToList();
            multipleApplications[0].Status = "Submitted";
            multipleApplications[1].Status = "InProgress";

            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/accounts/{_accountId}/legalentity/{_accountLegalEntityId}/applications")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonSerializer.Serialize(multipleApplications)));
        }

        [Then(@"the result should return Ok, with multiple applications with statuses of in progress and submitted")]
        public async Task ThenTheResultShouldReturnOkWithMultipleApplicationsWithStatusesOfInProgressAndSubmitted()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await _response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var result = JsonSerializer.Deserialize<IEnumerable<ApprenticeApplication>>(content, options);
            result.Should().NotBeNull();
            result.Count().Should().Be(2);
            var applications = result.ToList();
            applications[0].Status.Should().Be("Submitted");
            applications[1].Status.Should().Be("InProgress");
            applications[0].ApplicationId.Should().NotBe(applications[1].ApplicationId);
        }

    }
}

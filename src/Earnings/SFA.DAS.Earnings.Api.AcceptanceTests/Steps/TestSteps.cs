using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.Earnings.Api.AcceptanceTests.Steps
{
    [Binding]
    public class TestSteps
    {
        private readonly TestContext _context;
        private HttpResponseMessage _response;

        public TestSteps(TestContext context)
        {
            _context = context;
        }

        [Given(@"the Earnings Inner Api is (.*)")]
        public void GivenTheEmployerIncentivesInnerApiIs(string status)
        {
            _context.EarningsApi.MockServer
                .Given(
                    Request.Create().WithPath($"/ping")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)StatusCodeFromDescription(status))
                );
        }


        [Given(@"the Apprenticeships Inner Api is (.*)")]
        public void GivenTheApprenticeshipsInnerApiIs(string status)
        {
            _context.ApprenticeshipsApi.MockServer
                .Given(
                    Request.Create().WithPath($"/ping")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)StatusCodeFromDescription(status))
                );
        }


        [Given(@"the Collection Calendar Api is (.*)")]
        public void GivenTheCollectionCalendarApiIs(string status)
        {
            _context.CollectionCalendarApi.MockServer
                .Given(
                    Request.Create().WithPath($"/ping")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)StatusCodeFromDescription(status))
                );
        }

        [When(@"I request the service status")]
        public async Task WhenIRequestServiceStatus()
        {
            _response = await _context.OuterApiClient.GetAsync($"/health");
        }

        [Then(@"the result should be (.*)")]
        public void ThenTheResultShouldBe(string status)
        {
            if (status == "Healthy")
            {
                _response.IsSuccessStatusCode.Should().BeTrue();
            }
            else
            {
                _response.IsSuccessStatusCode.Should().BeFalse();
            }
        }

        public HttpStatusCode StatusCodeFromDescription(string status)
            =>
                (status == "Ok")
                    ? HttpStatusCode.OK
                    : HttpStatusCode.InternalServerError;

        public class HealthResponse
        {
            public string Status { get; set; }
        }
    }
}

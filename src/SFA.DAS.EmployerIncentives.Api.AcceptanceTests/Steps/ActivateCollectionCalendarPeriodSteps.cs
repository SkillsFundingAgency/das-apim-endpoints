using FluentAssertions;
using SFA.DAS.EmployerIncentives.Api.Models;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "ActivateCollectionCalendarPeriod")]
    public class ActivateCollectionCalendarPeriodSteps
    {
        private readonly TestContext _context;
        private HttpResponseMessage _response;

        public ActivateCollectionCalendarPeriodSteps(TestContext testContext)
        {
            _context = testContext;
        }

        [Given(@"the caller wants to activate a collection calendar period")]
        public void GivenTheCallerWantsToActivateACollectionCalendarPeriod()
        {
            
        }

        [Given(@"the Employer Incentives Api receives the collection calendar period activation request")]
        public void GivenTheEmployerIncentivesApiReceivesTheCollectionCalendarPeriodActivationRequest()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath("/collectionCalendar/period/active")
                        .UsingPatch())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                );
        }

        [When(@"the Outer Api receives the collection calendar period activation request")]
        public async Task WhenTheOuterApiReceivesTheCollectionCalendarPeriodActivationRequest()
        {
            var request = new ActivateCollectionCalendarPeriodRequest { CollectionPeriodNumber = 1, CollectionPeriodYear = 2020 };
            _response = await _context.OuterApiClient.PatchAsync("collectionCalendar/period/active", new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));
        }

        [Then(@"the response code of Ok is returned")]
        public void ThenTheResponseCodeOfOkIsReturned()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

    }
}

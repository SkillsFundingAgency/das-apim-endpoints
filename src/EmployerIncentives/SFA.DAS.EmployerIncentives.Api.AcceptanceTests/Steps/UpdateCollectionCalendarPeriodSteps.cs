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
    [Scope(Feature = "UpdateCollectionCalendarPeriod")]
    public class UpdateCollectionCalendarPeriodSteps
    {
        private readonly TestContext _context;
        private HttpResponseMessage _response;

        public UpdateCollectionCalendarPeriodSteps(TestContext testContext)
        {
            _context = testContext;
        }

        [Given(@"the caller wants to update a collection calendar period")]
        public void GivenTheCallerWantsToUpdateACollectionCalendarPeriod()
        {
            
        }

        [Given(@"the Employer Incentives Api receives the collection calendar period update request")]
        public void GivenTheEmployerIncentivesApiReceivesTheCollectionCalendarPeriodUpdateRequest()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath("/collectionPeriods")
                        .UsingPatch())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                );
        }

        [When(@"the Outer Api receives the collection calendar period update request")]
        public async Task WhenTheOuterApiReceivesTheCollectionCalendarPeriodUpdateRequest()
        {
            var request = new UpdateCollectionCalendarPeriodRequest { PeriodNumber = 1, AcademicYear = 2021 };
            _response = await _context.OuterApiClient.PatchAsync("collectionPeriods", new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));
        }

        [Then(@"the response code of Ok is returned")]
        public void ThenTheResponseCodeOfOkIsReturned()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

    }
}

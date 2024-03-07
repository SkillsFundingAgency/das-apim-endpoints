using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
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
    [Scope(Feature = "JobRequests")]
    public class JobRequestSteps
    {
        private readonly TestContext _context;
        private JobRequest _request;
        private HttpResponseMessage _response;
        private HttpStatusCode _innerResponseStatusCode;
        private readonly Fixture _fixture;

        public JobRequestSteps(TestContext context)
        {
            _fixture = new Fixture();
            _context = context;
        }

        [Given(@"the caller wants to start a RefreshLegalEntities EmployerIncentives Job")]
        public void GivenTheCallerWantsToStartArefreshLegalEntitiesJob()
        {
            _request = new JobRequest
            {
                Type = JobType.RefreshLegalEntities,
                 Data = new Dictionary<string, string>
                {
                    { "PageNumber", _fixture.Create<int>().ToString() },
                    { "PageSize", _fixture.Create<int>().ToString() }
                }
            };
        }

        [Given(@"the Employer Incentives Api receives the Job request")]
        public void GivenTheEmployerIncentivesApiShouldReceiveTheJobRequest()
        {
            _innerResponseStatusCode = HttpStatusCode.NoContent;

            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/jobs")
                        .UsingPut())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)_innerResponseStatusCode)                        
                );
        }

        [When(@"the Outer Api receives the Job request")]
        public async Task WhenTheOuterApiReceivesTheJobRequest()
        {
           _response = await  _context.OuterApiClient.PutAsJsonAsync($"jobs", _request);
        }

        [Then(@"the response of NoContent is returned")]
        public void ThenReturnNoContentToTheCaller()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}

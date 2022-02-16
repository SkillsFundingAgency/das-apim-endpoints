using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "GetApprovalsRegistration")]
    public class GetApprovalsRegistrationSteps
    {
        private readonly TestContext _context;
        private readonly long _commitmentsApprenticeshipId;
        private readonly string _innerApiResponse;
        private Fixture _f;

        public GetApprovalsRegistrationSteps(TestContext context)
        {
            _context = context;
            _f = new Fixture();

            _commitmentsApprenticeshipId = _f.Create<long>();

            var responseObject = new
            {
                RegistrationId = "3c1785bb-b7e9-4323-bf27-d73cf6edf7ac",
                Email = "paul@co-prime.com",
                HasApprenticeAssigned = true
            };

            _innerApiResponse = JsonConvert.SerializeObject(responseObject);
        }


        [Given(@"no approvals registration exists")]
        public void GivenNoApprovalsRegistrationExists()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath($"/approvals/{_commitmentsApprenticeshipId}/registration")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.NotFound)
                );
        }

        [Given(@"an approvals registration exists")]
        public void GivenAnApprovalsRegistrationExists()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath($"/approvals/{_commitmentsApprenticeshipId}/registration")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(_innerApiResponse)
                );
        }

        [When(@"retrieving approval registration details")]
        public Task WhenRetrievingApprovalRegistrationDetails()
        {
            return _context.OuterApiClient.Get($"approvals/{_commitmentsApprenticeshipId}/registration");
        }

        [Then(@"not found is returned")]
        public void ThenNotFoundIsReturned()
        {
            _context.OuterApiClient.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Then(@"Ok is returned")]
        public void ThenOkIsReturned()
        {
            _context.OuterApiClient.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Then(@"the response returns the inner api response")]
        public async Task ThenTheResponseContainsTheInnerApiResponse()
        {
            var response = await _context.OuterApiClient.Response.Content.ReadAsStringAsync();
            
            response.Should().NotBeNull();
            response.Should().Be(_innerApiResponse);
        }
    }
}
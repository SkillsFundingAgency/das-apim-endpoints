using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Common;
using Newtonsoft.Json;
using SFA.DAS.ApprenticeCommitments.Application.Commands.CreateApprenticeship;
using SFA.DAS.ApprenticeCommitments.InnerApi.Requests;
using TechTalk.SpecFlow;
using WireMock;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "AddApprenticeship")]
    public class AddApprenticeshipSteps
    {
        private readonly TestContext _context;
        private HttpResponseMessage _response;
        private CreateApprenticeshipCommand _request;
        private Dictionary<string, string> _errors;

        public AddApprenticeshipSteps(TestContext context)
        {
            _context = context;
            _errors = new Dictionary<string, string>();
            _errors.Add("apprenticeshipId", "Not valid");
            _errors.Add("email", "Not valid");
        }

        [Given(@"apprenticeship details are valid")]
        public void GivenApprenticeshipDetailsAreValid()
        {
            _request = new CreateApprenticeshipCommand
            {
                ApprenticeshipId = 1020,
                Email = "Test@Test.com"
            };
        }

        [Given(@"apprenticeship details are not valid")]
        public void GivenApprenticeshipDetailsAreNotValid()
        {
            _request = new CreateApprenticeshipCommand
            {
                ApprenticeshipId = 0,
                Email = "noemail"
            };
        }

        [Given(@"the inner api is ready")]
        public void GivenTheInnerApiIsReady()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/apprenticeships")
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.Accepted)
                );
        }

        [Given(@"the inner api will return a bad request")]
        public void GivenTheInnerApiWillReturnABadRequest()
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/apprenticeships")
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.BadRequest)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonConvert.SerializeObject(_errors))
                );
        }

        [When(@"the apprenticeship is posted")]
        public async Task WhenTheApprenticeshipIsPosted()
        {
            await _context.OuterApiClient.Post("apprenticeships", _request);
        }

        [Then(@"the result should be Accepted")]
        public void ThenTheResultShouldBeAccepted()
        {
            _context.OuterApiClient.Response.StatusCode.Should().Be(HttpStatusCode.Accepted);
        }

        [Then(@"the result should be Bad Request")]
        public void ThenTheResultShouldBeBadRequest()
        {
            _context.OuterApiClient.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Then(@"the request to the inner api was mapped correctly")]
        public void ThenTheRequestToTheInnerApiWasMappedCorrectly()
        {
            var logs = _context.InnerApi.MockServer.LogEntries;
            logs.Count().Should().Be(1);
            var log = logs.First();
            
            var innerApiRequest = JsonConvert.DeserializeObject<CreateApprenticeshipRequestData>(log.RequestMessage.Body);
            innerApiRequest.Should().NotBeNull();
            innerApiRequest.RegistrationId.Should().NotBe(Guid.Empty);
            innerApiRequest.Email.Should().Be(_request.Email);
            innerApiRequest.ApprenticeshipId.Should().Be(_request.ApprenticeshipId);
        }

        [Then(@"the result should contain errors")]
        public async Task ThenTheResultShouldContainErrors()
        {
            var content = await _context.OuterApiClient.Response.Content.ReadAsStringAsync();

            var errors = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);

            errors.Should().BeEquivalentTo(_errors);
        }

    }
}
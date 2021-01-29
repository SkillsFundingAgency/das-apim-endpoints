using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.ApprenticeCommitments.Apis.ApprenticeLoginApi;
using SFA.DAS.ApprenticeCommitments.Apis.CommitmentsV2InnerApi;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using SFA.DAS.ApprenticeCommitments.Application.Commands.CreateApprenticeship;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "AddApprenticeship")]
    public class AddApprenticeshipSteps
    {
        private readonly TestContext _context;
        private CreateApprenticeshipCommand _request;
        private Dictionary<string, string> _errors;
        private SendInvitationRequestData loginApiRequest;

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
                Email = "Test@Test.com",
                Organisation = "OrganisationName",
            };
        }

        [Given(@"apprenticeship details are not valid")]
        public void GivenApprenticeshipDetailsAreNotValid()
        {
            _request = new CreateApprenticeshipCommand
            {
                ApprenticeshipId = 0,
                Email = "noemail",
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

        [Given(@"the apprentice login api is ready")]
        public void GivenTheApprenticeLoginApiIsReady()
        {
            _context.LoginApi.MockServer
                .Given(
                    Request.Create().WithPath($"/invitations/{_context.LoginConfig.IdentityServerClientId}")
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                );
        }

        [Given(@"the Approvals API is ready")]
        public void GivenTheApprovalsAPIIsReady()
        {
            _context.CommitmentsV2InnerApi.MockServer
                .Given(
                    Request.Create().WithPath($"/api/apprenticeships/1020")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonConvert.SerializeObject(new ApprenticeshipResponse
                        {
                            FirstName = "GivenName",
                            LastName = "FamilyName",
                            CourseName = "ApprenticeshipName",
                        }))
                ); ;
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

        [Then(@"the invitation was sent successfully")]
        public void ThenTheInvitationWasSentSuccessfully()
        {
            var logs = _context.LoginApi.MockServer.LogEntries;
            logs.Count().Should().Be(1);
            var log = logs.First();

            loginApiRequest = JsonConvert.DeserializeObject<SendInvitationRequestData>(log.RequestMessage.Body);
            loginApiRequest.Should().NotBeNull();
            loginApiRequest.SourceId.Should().NotBe(Guid.Empty);
            loginApiRequest.Email.Should().Be(_request.Email);
            loginApiRequest.GivenName.Should().Be("GivenName");
            loginApiRequest.FamilyName.Should().Be("FamilyName");
            loginApiRequest.OrganisationName.Should().Be("OrganisationName");
            loginApiRequest.ApprenticeshipName.Should().Be("ApprenticeshipName");
            loginApiRequest.Callback.Should().Be(_context.LoginConfig.CallbackUrl);
            loginApiRequest.UserRedirect.Should().Be(_context.LoginConfig.RedirectUrl);
        }
    }
}
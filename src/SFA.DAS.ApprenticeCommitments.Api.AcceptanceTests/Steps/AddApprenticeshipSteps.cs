using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.ApprenticeCommitments.Apis.ApprenticeLoginApi;
using SFA.DAS.ApprenticeCommitments.Apis.CommitmentsV2InnerApi;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using SFA.DAS.ApprenticeCommitments.Application.Commands.CreateApprenticeship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using WireMock.Matchers;
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
        private IEnumerable<Apis.CommitmentsV2InnerApi.ApprenticeshipResponse> _approvedApprenticeships;

        public AddApprenticeshipSteps(TestContext context)
        {
            _context = context;

            _context.InnerApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath("/apprenticeships")
                        .UsingPost()
                        .WithBody(new JmesPathMatcher(
                            "ApprenticeshipId != `0` && contains(Email, '@')"))
                      )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.Accepted)
                            );

            _context.InnerApi.MockServer
                .Given(
                    Request.Create().WithPath("/apprenticeships")
                        .UsingPost()
                        .WithBody(new JmesPathMatcher(
                            "!contains(Email, '@')"))
                      )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.BadRequest)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody("{'email':'Not valid'}")
                            );

            _context.LoginApi.MockServer
                .Given(
                    Request.Create().WithPath($"/invitations/{_context.LoginConfig.IdentityServerClientId}")
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                            );
        }

        [Given("the following apprenticeships have been approved")]
        public void GivenTheFollowingApprenticeshipsHaveBeenApproved(Table table)
        {
            _approvedApprenticeships = table.CreateSet<Apis.CommitmentsV2InnerApi.ApprenticeshipResponse>();

            foreach (var approval in _approvedApprenticeships)
            {
                _context.CommitmentsV2InnerApi.MockServer
                    .Given(
                        Request.Create()
                            .WithPath($"/api/apprenticeships/{approval.Id}")
                            .UsingGet()
                          )
                    .RespondWith(
                        Response.Create()
                            .WithStatusCode((int)HttpStatusCode.OK)
                            .WithHeader("Content-Type", "application/json")
                            .WithBody(JsonConvert.SerializeObject(approval))
                                );
            }
        }

        [When("the following apprenticeship is posted")]
        public async Task WhenTheFollowingApprenticeshipIsPosted(Table table)
        {
            _request = table.CreateInstance<CreateApprenticeshipCommand>();
            await _context.OuterApiClient.Post("apprenticeships", _request);
        }

        [Then("the inner API was called successfully")]
        public void ThenTheRequestToTheInnerApiWasMappedCorrectly()
        {
            _context.OuterApiClient.Response.StatusCode
                .Should().Be(HttpStatusCode.Accepted);

            var logs = _context.InnerApi.MockServer.LogEntries;
            logs.Should().HaveCount(1);

            var innerApiRequest = JsonConvert.DeserializeObject<CreateApprenticeshipRequestData>(
                logs.First().RequestMessage.Body);

            innerApiRequest.Should().NotBeNull();
            innerApiRequest.RegistrationId.Should().NotBe(Guid.Empty);
            innerApiRequest.Email.Should().Be(_request.Email);
            innerApiRequest.ApprenticeshipId.Should().Be(_request.ApprenticeshipId);
            innerApiRequest.EmployerName.Should().Be(_request.EmployerName);
            innerApiRequest.EmployerAccountLegalEntityId.Should().Be(_request.EmployerAccountLegalEntityId);
            innerApiRequest.TrainingProviderName.Should().Be("Provisional Training Provider Name");
        }

        [Then("the inner API should return these errors")]
        public async Task ThenTheInnerAPIShouldReturnTheseErrors(Table table)
        {
            _context.OuterApiClient.Response.StatusCode
                .Should().Be(HttpStatusCode.BadRequest);

            var expectedErrors = table.Rows.ToDictionary(r => r[0], r => r[1]);

            var content = await _context.OuterApiClient.Response.Content.ReadAsStringAsync();
            var errors = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
            errors.Should().BeEquivalentTo(expectedErrors);
        }

        [Then("the invitation was sent successfully")]
        public void ThenTheInvitationWasSentSuccessfully()
        {
            var expectedCommitment = _approvedApprenticeships.First(
                x => x.Id == _request.ApprenticeshipId);

            var logs = _context.LoginApi.MockServer.LogEntries;
            logs.Should().HaveCount(1);

            var loginApiRequest = JsonConvert.DeserializeObject<SendInvitationRequestData>(
                logs.First().RequestMessage.Body);
            loginApiRequest.Should().NotBeNull();
            loginApiRequest.SourceId.Should().NotBe(Guid.Empty);
            loginApiRequest.Email.Should().Be(_request.Email);
            loginApiRequest.GivenName.Should().Be(expectedCommitment.FirstName);
            loginApiRequest.FamilyName.Should().Be(expectedCommitment.LastName);
            loginApiRequest.OrganisationName.Should().Be(_request.EmployerName);
            loginApiRequest.ApprenticeshipName.Should().Be(expectedCommitment.CourseName);
            loginApiRequest.Callback.Should().Be(_context.LoginConfig.CallbackUrl);
            loginApiRequest.UserRedirect.Should().Be(_context.LoginConfig.RedirectUrl);
        }

        [Then("the invitation was not sent")]
        public void ThenTheInvitationWasNotSent()
        {
            _context.LoginApi.MockServer.LogEntries.Should().BeEmpty();
        }
    }
}
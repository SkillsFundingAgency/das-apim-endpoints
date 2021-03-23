using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.ApprenticeCommitments.Apis.ApprenticeLoginApi;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using SFA.DAS.ApprenticeCommitments.Application.Commands.SendInvitationReminders;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using ApprenticeshipResponse = SFA.DAS.ApprenticeCommitments.Apis.CommitmentsV2InnerApi.ApprenticeshipResponse;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "SendInvitationReminders")]
    public class SendInvitationRemindersSteps
    {
        private readonly TestContext _context;
        private SendInvitationRemindersCommand _command;
        private Fixture _fixture;
        private List<RegistrationsRemindersInvitationsResponse.Registration> _registrationsRequiringReminders;
        private ApprenticeshipResponse _commitmentsApprenticeshipResponse;

        public SendInvitationRemindersSteps(TestContext context)
        {
            _context = context;
            _fixture = new Fixture();
            _command = _fixture.Create<SendInvitationRemindersCommand>();

            _commitmentsApprenticeshipResponse = _fixture.Create<ApprenticeshipResponse>();

            _context.InnerApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath($"/registrations/*/reminder")
                        .UsingPost()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                );

            _context.LoginApi.MockServer
                .Given(
                    Request.Create().WithPath($"/invitations/*")
                        .UsingPost())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                );
        }

        [Given(@"there are no reminders")]
        public void GivenThereAreNoReminders()
        {
            CreateRemindersResponse(new List<RegistrationsRemindersInvitationsResponse.Registration>());
        }

        [Given(@"there is one reminder")]
        public void GivenThereIsOneReminder()
        {
            _registrationsRequiringReminders = _fixture.CreateMany<RegistrationsRemindersInvitationsResponse.Registration>(1).ToList();
            CreateRemindersResponse(_registrationsRequiringReminders);
        }

        [Given(@"there are reminders")]
        public void GivenThereAreReminders()
        {
            _registrationsRequiringReminders = _fixture.CreateMany<RegistrationsRemindersInvitationsResponse.Registration>().ToList();
            CreateRemindersResponse(_registrationsRequiringReminders);
        }

        [Given(@"the course names are found")]
        public void GivenTheCourseNamesAreFound()
        {
            foreach (var r in _registrationsRequiringReminders)
            {
                _commitmentsApprenticeshipResponse.CourseName = $"Course for {r.ApprenticeshipId}";
                _context.CommitmentsV2InnerApi.MockServer
                    .Given(
                        Request.Create()
                            .WithPath($"/api/apprenticeships/{r.ApprenticeshipId}")
                            .UsingGet()
                    )
                    .RespondWith(
                        Response.Create()
                            .WithStatusCode((int) HttpStatusCode.OK)
                            .WithBodyAsJson(_commitmentsApprenticeshipResponse)
                    );
            }
        }

        [When(@"the scheduled job starts process for sending reminders")]
        public Task WhenTheScheduledJobStartsProcessForSendingReminders()
        {
            return _context.OuterApiClient.Post("/registrations/reminders", _command);
        }

        [Then(@"return an ok response")]
        public void ThenReturnAnOkResponse()
        {
            _context.OuterApiClient.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Then(@"no invitations are sent")]
        public void ThenNoInvitationsAreSent()
        {
            var logs = _context.LoginApi.MockServer.LogEntries;
            logs.Should().HaveCount(0);
        }

        [Then(@"the invitation is sent with expected values")]
        public void ThenTheInvitationIsSentWithExpectedValues()
        {
            var reminder = _registrationsRequiringReminders.First();
            var logs = _context.LoginApi.MockServer.LogEntries;
            logs.Should().HaveCount(1);

            var innerApiRequest = JsonConvert.DeserializeObject<SendInvitationRequestData>(
                logs.First().RequestMessage.Body);

            innerApiRequest.Should().NotBeNull();
            innerApiRequest.SourceId.Should().Be(reminder.ApprenticeId);
            innerApiRequest.Email.Should().Be(reminder.Email);
            innerApiRequest.GivenName.Should().Be(reminder.FirstName);
            innerApiRequest.FamilyName.Should().Be(reminder.LastName);
            innerApiRequest.ApprenticeshipName.Should().Be(_commitmentsApprenticeshipResponse.CourseName);
            innerApiRequest.OrganisationName.Should().Be(reminder.EmployerName);
            innerApiRequest.UserRedirect.Should().Be(_context.LoginConfig.RedirectUrl);
            innerApiRequest.Callback.Should().Be(_context.LoginConfig.CallbackUrl);
        }

        [Then(@"the registration is marked as sent")]
        public void ThenTheRegistrationIsMarkedAsSent()
        {
            var reminder = _registrationsRequiringReminders.First();
            var log = _context.InnerApi.MockServer.LogEntries.LastOrDefault();
            log.Should().NotBeNull();

            var url = log.RequestMessage.Url;
            url.Should().EndWith($"registrations/{reminder.ApprenticeId}/reminder");

            var body = log.RequestMessage.Body;
            var request = JsonConvert.DeserializeObject<InvitationReminderSentData>(body);
            request.SentOn.Should().Be(_command.SendNow);
        }

        [Then(@"all invitations are sent")]
        public void ThenAllInvitationsAreSent()
        {
            var logs = _context.LoginApi.MockServer.LogEntries;
            logs.Should().HaveCount(_registrationsRequiringReminders.Count);
        }

        private void CreateRemindersResponse(IEnumerable<RegistrationsRemindersInvitationsResponse.Registration> registrations)
        {
            _context.InnerApi.MockServer
                .Given(
                    Request.Create()
                        .WithPath($"/registrations/reminders")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithBodyAsJson(new RegistrationsRemindersInvitationsResponse
                        {
                            Registrations = registrations
                        })
                );
        }
    }
}
using System;
using System.Collections.Generic;
using System.Net;
using AutoFixture;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace SFA.DAS.ApprenticeCommitments.MockApis
{
    public class ApprenticeCommitmentsInnerApiBuilder
    {
        private readonly WireMockServer _server;
        private readonly Fixture _fixture;
        private IEnumerable<RegistrationsRemindersInvitationsResponse.Registration> _registrationsInNeedOfReminders;
        private readonly ApprenticeshipResponse _existingApprenticship;
        private readonly ApprenticeshipResponse _latestConfirmedApprenticship;

        public ApprenticeCommitmentsInnerApiBuilder(int port)
        {
            _fixture = new Fixture();
            _server = WireMockServer.StartWithAdminInterface(port, true);

            _registrationsInNeedOfReminders = _fixture.CreateMany<RegistrationsRemindersInvitationsResponse.Registration>();
            _existingApprenticship = _fixture.Create<ApprenticeshipResponse>();
            _latestConfirmedApprenticship = _fixture.Create<ApprenticeshipResponse>();
        }

        public static ApprenticeCommitmentsInnerApiBuilder Create(int port)
        {
            return new ApprenticeCommitmentsInnerApiBuilder(port);
        }

        public WireMockServer Build()
        {
            Console.WriteLine($"Apprentice Commitments Inner Fake Api Running ({_server.Urls[0]})");
            return _server;
        }

        public ApprenticeCommitmentsInnerApiBuilder WithPing()
        {
            _server
                .Given(
                    Request.Create()
                        .WithPath($"/ping")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                );

            return this;
        }

        public ApprenticeCommitmentsInnerApiBuilder WithAnyNewApprenticeship()
        {
            _server
                .Given(
                    Request.Create()
                        .WithPath("/apprenticeships")
                        .UsingPost()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int) HttpStatusCode.Accepted)
                );

            _server
                .Given(
                    Request.Create().WithPath("/apprenticeships/change").UsingPost()
                )
                .RespondWith(
                    Response.Create().WithStatusCode((int) HttpStatusCode.Accepted)
                );

            return this;
        }

        public ApprenticeCommitmentsInnerApiBuilder WithReminderSent()
        {
            _server
                .Given(
                    Request.Create()
                        .WithPath("/registrations/*/reminder")
                        .UsingPost()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.Accepted)
                );

            return this;
        }

        internal ApprenticeCommitmentsInnerApiBuilder WithExistingApprenticeship()
        {
            _server
                .Given(
                    Request.Create()
                        .WithPath("/apprentices/e3bab44c-469b-4609-b926-afffef41f451/apprenticeships/100")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                    .WithStatusCode((int)HttpStatusCode.OK)
                    .WithBodyAsJson(_existingApprenticship)
                );

            return this;
        }

        public ApprenticeCommitmentsInnerApiBuilder WithRegistrationReminders()
        {
            _server
                .Given(
                    Request.Create()
                        .WithPath("/registrations/reminders")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithBodyAsJson(new RegistrationsRemindersInvitationsResponse{ Registrations = _registrationsInNeedOfReminders })
                );

            return this;
        }

        public ApprenticeCommitmentsInnerApiBuilder WithRegistrationSeen()
        {
            _server
                .Given(
                    Request.Create()
                        .WithPath("/registrations/*/firstseen")
                        .UsingPost()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.Accepted)
                );

            return this;
        }

        public ApprenticeCommitmentsInnerApiBuilder WithHowApprenticeshipWillBeDelivered()
        {
            _server
                .Given(
                    Request.Create()
                        .WithPath("/apprentices/*/apprenticeships/*/howapprenticeshipwillbedeliveredconfirmation")
                        .UsingPost()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                );

            return this;
        }

        public ApprenticeCommitmentsInnerApiBuilder WithLatestConfirmedApprenticeship()
        {
            _server
                .Given(
                    Request.Create()
                        .WithPath("/apprentices/*/apprenticeships/*/latest-confirmed-details")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithBodyAsJson(_latestConfirmedApprenticship)
                );

            return this;
        }
    }
}
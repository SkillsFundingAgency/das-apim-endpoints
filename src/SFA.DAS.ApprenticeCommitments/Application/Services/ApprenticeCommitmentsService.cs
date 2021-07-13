using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using SFA.DAS.ApprenticeCommitments.Application.Commands.VerifyIdentityRegistration;
using SFA.DAS.ApprenticeCommitments.Application.Queries.Registration;
using SFA.DAS.ApprenticeCommitments.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Application.Services
{
    public class ApprenticeCommitmentsService
    {
        private readonly IInternalApiClient<ApprenticeCommitmentsConfiguration> _client;

        public ApprenticeCommitmentsService(IInternalApiClient<ApprenticeCommitmentsConfiguration> client)
            => _client = client;

        public Task<bool> IsHealthy() => HealthCheck.IsHealthy(_client);

        public Task CreateApprenticeship(CreateApprenticeshipRequestData data)
        {
            return _client.Post(new CreateApprenticeshipRequest
            {
                Data = data
            });
        }

        public Task ChangeApprenticeship(ChangeApprenticeshipRequestData data)
            => _client.Post(new ChangeApprenticeshipRequest { Data = data });

        internal Task ChangeEmailAddress(Guid apprenticeshipId, string email)
        {
            return _client.Post<ChangeEmailAddressResponse>(
                new ChangeEmailAddressRequest(apprenticeshipId)
                {
                    Data = new ChangeEmailAddressRequestData
                    {
                        Email = email,
                    }
                });
        }

        public Task<RegistrationResponse> GetRegistration(Guid id) =>
            _client.Get<RegistrationResponse>(new GetRegistrationDetailsRequest(id));

        public Task VerifyRegistration(VerifyIdentityRegistrationCommand command)
        {
            return _client.Post(new VerifyRegistrationRequest
            {
                Data = new VerifyRegistrationRequestData
                {
                    ApprenticeId = command.ApprenticeId,
                    UserIdentityId = command.UserIdentityId,
                    FirstName = command.FirstName,
                    LastName = command.LastName,
                    Email = command.Email,
                    DateOfBirth = command.DateOfBirth
                }
            });
        }

        public Task<RegistrationsRemindersInvitationsResponse> GetReminderRegistrations(DateTime invitationCutOffTime) => 
            _client.Get<RegistrationsRemindersInvitationsResponse>(new GetRegistrationsNeedingRemindersRequest(invitationCutOffTime));

        public Task InvitationReminderSent(Guid apprenticeId, DateTime sentOn)
        {
            return _client.Post(
                new InvitationReminderSentRequest(apprenticeId)
                {
                    Data = new InvitationReminderSentData
                    {
                        SentOn = sentOn
                    }
                });
        }
    }
}
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using SFA.DAS.ApprenticeCommitments.Application.Services.ApprenticeLogin;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.SendInvitationReminders
{
    public class SendInvitationRemindersCommandHandler : IRequestHandler<SendInvitationRemindersCommand>
    {
        private readonly ApprenticeCommitmentsService _apprenticeCommitmentsService;
        private readonly ApprenticeLoginService _apprenticeLoginService;
        private readonly CommitmentsV2Service _commitmentsV2Service;
        private readonly ILogger<SendInvitationRemindersCommandHandler> _logger;

        public SendInvitationRemindersCommandHandler(
            ApprenticeCommitmentsService apprenticeCommitmentsService,
            ApprenticeLoginService apprenticeLoginService, 
            CommitmentsV2Service commitmentsV2Service,
            ILogger<SendInvitationRemindersCommandHandler> logger)
        {
            _apprenticeCommitmentsService = apprenticeCommitmentsService;
            _apprenticeLoginService = apprenticeLoginService;
            _commitmentsV2Service = commitmentsV2Service;
            _logger = logger;
        }

        public async Task<Unit> Handle(SendInvitationRemindersCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting Registrations which are due a reminder");
            var list = (await _apprenticeCommitmentsService.GetReminderRegistrations()).Registrations;

            // This just wouldn't work 
            //Parallel.ForEach(list, new ParallelOptions {MaxDegreeOfParallelism = 4}, async registration =>
            //{
            //    await SendReminder(registration);
            //});

            await Task.WhenAll(list.Select(x=>SendReminder(x, command.SendNow)));

            return Unit.Value;
        }

        private async Task SendReminder(RegistrationsRemindersInvitationsResponse.Registration registration, DateTime sentOn)
        {
            try
            {
                _logger.LogInformation($"Getting Apprenticeship Details for ApprenticeshipId {registration.ApprenticeshipId}");
                var apprenticeship =
                    await _commitmentsV2Service.GetApprenticeshipDetails(registration.ApprenticeshipId);

                _logger.LogInformation($"Sending Invitation for Apprentice {registration.ApprenticeId}");
                await _apprenticeLoginService.SendInvitation(new SendInvitationModel
                {
                    SourceId = registration.ApprenticeId,
                    Email = registration.Email,
                    GivenName = registration.FirstName,
                    FamilyName = registration.LastName,
                    OrganisationName = registration.EmployerName,
                    ApprenticeshipName = apprenticeship.CourseName
                });

                _logger.LogInformation($"Updating Registration for Apprentice {registration.ApprenticeId}");
                await _apprenticeCommitmentsService.InvitationReminderSent(registration.ApprenticeId, sentOn);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error Sending a Reminder for Apprentice {registration.ApprenticeId}");
            }
        }
    }
}
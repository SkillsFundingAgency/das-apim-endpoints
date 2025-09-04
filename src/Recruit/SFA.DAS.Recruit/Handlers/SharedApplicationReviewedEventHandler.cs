using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.Recruit.Application.Vacancies.Queries.GetVacancyById;
using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.Domain.EmailTemplates;
using SFA.DAS.Recruit.Domain.Vacancy;
using SFA.DAS.Recruit.Enums;
using SFA.DAS.Recruit.Events;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Handlers;
public class SharedApplicationReviewedEventHandler(IMediator mediator,
    ILogger<SharedApplicationReviewedEventHandler> logger,
    IRecruitApiClient<RecruitApiConfiguration> apiClient,
    INotificationService notificationService,
    EmailEnvironmentHelper emailEnvironmentHelper) : INotificationHandler<SharedApplicationReviewedEvent>
{
    public async Task Handle(SharedApplicationReviewedEvent @event, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetVacancyByIdQuery(@event.VacancyId), cancellationToken);
        if (result == GetVacancyByIdQueryResult.None)
        {
            logger.LogError(
                "SharedApplicationReviewedEventHandler: Vacancy not found '{VacancyId}' ({VacancyReference})",
                @event.VacancyId,
                @event.VacancyReference);
            return;
        }

        var vacancy = result.Vacancy;
        if (vacancy?.OwnerType != OwnerType.Provider || vacancy.TrainingProvider?.Ukprn is null)
        {
            return; // Nothing to do if not provider-owned or missing provider info
        }

        var users = await GetEligibleUsersAsync(vacancy.TrainingProvider.Ukprn.Value);
        if (users is null || users.Count == 0)
        {
            return; // No users to notify
        }

        var emailCommands = BuildEmailCommands(users, vacancy, @event);
        if (emailCommands.Count == 0)
        {
            return; // No email commands to send
        }

        await Task.WhenAll(emailCommands.Select(notificationService.Send));
    }

    private async Task<List<RecruitUserApiResponse>> GetEligibleUsersAsync(long ukprn)
    {
        var users = await apiClient.GetAll<RecruitUserApiResponse>(
            new GetProviderRecruitUserNotificationPreferencesApiRequest(
                ukprn, NotificationTypes.SharedApplicationReviewedByEmployer));

        return users?
            .Where(u => u.NotificationPreferences.EventPreferences.Any(p =>
                p.Event == NotificationTypes.SharedApplicationReviewedByEmployer &&
                p.Scope == NotificationScope.OrganisationVacancies))
            .ToList() ?? [];
    }

    private List<SendEmailCommand> BuildEmailCommands(
        IEnumerable<RecruitUserApiResponse> users,
        Vacancy vacancy,
        SharedApplicationReviewedEvent notification)
    {
        return users.Select(user =>
        {
            var email = new SharedApplicationsReturnedEmailTemplate(
                emailEnvironmentHelper.ApplicationReviewSharedEmailTemplatedId,
                user.Email,
                vacancy.Title,
                user.Name,
                vacancy.VacancyReference?.ToString() ?? string.Empty,
                vacancy.EmployerName,
                string.Format(emailEnvironmentHelper.ManageVacancyProviderUrl, vacancy.TrainingProvider?.Ukprn, notification.VacancyId),
                string.Format(emailEnvironmentHelper.NotificationsSettingsProviderUrl, vacancy.TrainingProvider?.Ukprn)
            );
            return new SendEmailCommand(email.TemplateId, email.RecipientAddress, email.Tokens);
        }).ToList();
    }
}
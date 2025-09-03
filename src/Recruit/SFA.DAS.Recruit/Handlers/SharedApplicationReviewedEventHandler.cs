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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Handlers;
public class SharedApplicationReviewedEventHandler(IMediator mediator,
    ILogger<SharedApplicationReviewedEventHandler> logger,
    IRecruitApiClient<RecruitApiConfiguration> apiClient,
    EmailEnvironmentHelper emailEnvironmentHelper,
    INotificationService notificationService) : INotificationHandler<SharedApplicationReviewedEvent>
{
    public async Task Handle(SharedApplicationReviewedEvent @notification, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetVacancyByIdQuery(@notification.VacancyId), cancellationToken);
        if (result == GetVacancyByIdQueryResult.None)
        {
            logger.LogError("SharedApplicationReviewedEventHandler: Vacancy not found '{VacancyId}' ({VacancyReference})", @notification.VacancyId, @notification.VacancyReference);
            return;
        }

        var vacancy = result.Vacancy;
        if (vacancy is { OwnerType: OwnerType.Provider })
        {
            var users = await apiClient.GetAll<RecruitUserApiResponse>(
                new GetProviderRecruitUserNotificationPreferencesApiRequest(vacancy.TrainingProvider!.Ukprn!.Value,
                    NotificationTypes.SharedApplicationReviewedByEmployer));

            var emailTasks = users?
                .Where(x =>
                    x.NotificationPreferences.EventPreferences.Any(p =>
                        p.Event == NotificationTypes.SharedApplicationReviewedByEmployer &&
                        p.Scope == NotificationScope.OrganisationVacancies))
                .Select(x => new SharedApplicationsReturnedEmailTemplate(
                    emailEnvironmentHelper.ApplicationReviewSharedEmailTemplatedId,
                    x.Email,
                    vacancy.Title,
                    x.Name,
                    vacancy.VacancyReference!.Value.ToString(),
                    vacancy.EmployerName,
                    string.Format(emailEnvironmentHelper.ManageVacancyProviderUrl, vacancy.TrainingProvider!.Ukprn!.Value, notification.VacancyId),
                    string.Format(emailEnvironmentHelper.NotificationsSettingsProviderUrl, vacancy.TrainingProvider!.Ukprn!.Value)
                ))
                .Select(email => new SendEmailCommand(email.TemplateId, email.RecipientAddress, email.Tokens))
                .Select(notificationService.Send).ToList();

            if (emailTasks?.Count != 0)
            {
                await Task.WhenAll(emailTasks!);
            }
        }
    }
}
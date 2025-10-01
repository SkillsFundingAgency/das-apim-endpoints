using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.Recruit.Application.Vacancies.Queries.GetVacancyById;
using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.Domain.EmailTemplates;
using SFA.DAS.Recruit.Enums;
using SFA.DAS.Recruit.Events;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Handlers;

public class ApplicationSubmittedEventHandler(
    IMediator mediator,
    ILogger<ApplicationSubmittedEventHandler> logger,
    IRecruitApiClient<RecruitApiConfiguration> apiClient,
    EmailEnvironmentHelper emailHelper,
    INotificationService notificationService) : INotificationHandler<ApplicationSubmittedEvent>
{
    public async Task Handle(ApplicationSubmittedEvent @event, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetVacancyByIdQuery(@event.VacancyId), cancellationToken);
        if (result == GetVacancyByIdQueryResult.None)
        {
            logger.LogError("ApplicationSubmittedEventHandler: Vacancy not found '{VacancyId}'", @event.VacancyId);
            return;
        }
        var vacancy = result.Vacancy;

        if (vacancy.AccountId is not long employerAccountId)
        {
            logger.LogError("ApplicationSubmittedEventHandler: Vacancy does not have an associated employer account '{VacancyId}'", @event.VacancyId);
            return;
        }
        if (vacancy.VacancyReference is not long vacancyReference)
        {
            logger.LogError("ApplicationSubmittedEventHandler: Vacancy does not have a VacancyReference '{VacancyId}'", @event.VacancyId);
            return;
        }

        var location = vacancy.EmployerLocationOption == AvailableWhere.AcrossEngland
            ? "Recruiting nationally"
            : EmailTemplateAddressExtension.GetEmploymentLocationCityNames(vacancy.EmployerLocations);

        var users = await apiClient.GetAll<RecruitUserApiResponse>(
            new GetEmployerRecruitUserNotificationPreferencesApiRequest(employerAccountId,
                NotificationTypes.ApplicationSubmitted));

        var usersToNotify = users?
            .Where(x => x.NotificationPreferences.EventPreferences.Any(p => 
                p.Frequency == NotificationFrequency.Immediately &&
                (p.Scope == NotificationScope.OrganisationVacancies || 
                    (p.Scope == NotificationScope.UserSubmittedVacancies && x.Id == vacancy.SubmittedByUserId))))
            .ToList();

        var emailTasks = usersToNotify?
            .Select(x => new ApplicationSubmittedEmailTemplate(
                emailHelper.ApplicationSubmittedTemplateId,
                x.Email,
                vacancy.Title,
                x.FirstName,
                vacancyReference.ToString(),
                vacancy.EmployerName,
                location,
                string.Format(emailHelper.ManageAdvertUrl, employerAccountId, vacancy.Id),
                string.Format(emailHelper.NotificationsSettingsEmployerUrl, employerAccountId)))
            .Select(email => new SendEmailCommand(email.TemplateId, email.RecipientAddress, email.Tokens))
            .Select(notificationService.Send).ToList();

        if (emailTasks?.Count > 0)
        {
            await Task.WhenAll(emailTasks!);
        }
    }
}

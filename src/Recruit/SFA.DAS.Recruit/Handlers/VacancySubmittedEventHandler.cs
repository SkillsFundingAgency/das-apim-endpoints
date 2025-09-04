using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Handlers;

public class VacancySubmittedEventHandler(
    IMediator mediator,
    ILogger<VacancySubmittedEventHandler> logger,
    IRecruitApiClient<RecruitApiConfiguration> apiClient,
    EmailEnvironmentHelper emailHelper,
    INotificationService notificationService): INotificationHandler<VacancySubmittedEvent>
{
    public async Task Handle(VacancySubmittedEvent @event, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetVacancyByIdQuery(@event.VacancyId), cancellationToken);
        if (result == GetVacancyByIdQueryResult.None)
        {
            logger.LogError("VacancySubmittedEventHandler: Vacancy not found '{VacancyId}' ({VacancyReference})", @event.VacancyId, @event.VacancyReference);
            return;
        }

        var vacancy = result.Vacancy;
        if (vacancy is { OwnerType: OwnerType.Provider, ReviewRequestedByUserId: not null })
        {
            // TODO: this should be pulled out into another service
            var users = await apiClient.GetAll<RecruitUserApiResponse>(
                new GetProviderRecruitUserNotificationPreferencesApiRequest(vacancy.TrainingProvider!.Ukprn!.Value,
                    NotificationTypes.VacancyApprovedOrRejected));

            var location = vacancy.EmployerLocationOption == AvailableWhere.AcrossEngland
                ? "Recruiting nationally"
                : EmailTemplateAddressExtension.GetEmploymentLocationCityNames(vacancy.EmployerLocations);

            var emailTasks = users?
                .Where(x =>
                    // either the provider user who submitted the vacancy to the employer
                    x.Id == vacancy.ReviewRequestedByUserId
                    // or any other user interested in all org vacancies
                    || x.NotificationPreferences.EventPreferences.Any(p =>
                        p.Event == NotificationTypes.VacancyApprovedOrRejected &&
                        p.Scope == NotificationScope.OrganisationVacancies))
                .Select(x => new VacancySubmittedEmailTemplate(
                    emailHelper.AdvertApprovedByDfeTemplateId,
                    x.Email,
                    vacancy.Title,
                    x.FirstName,
                    vacancy.VacancyReference!.Value.ToString(),
                    vacancy.EmployerName,
                    location,
                    string.Format(emailHelper.NotificationsSettingsProviderUrl, vacancy.TrainingProvider.Ukprn)
                ))
                .Select(email => new SendEmailCommand(email.TemplateId, email.RecipientAddress, email.Tokens))
                .Select(notificationService.Send).ToList();

            if (emailTasks?.Count > 0)
            {
                await Task.WhenAll(emailTasks!);
            }
        }
    }
}
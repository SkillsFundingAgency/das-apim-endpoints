using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Application.Vacancies.Queries.GetVacancyByReference;
using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.Enums;
using SFA.DAS.Recruit.Events;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Handlers
{
    public class ApplicationSubmittedEventHandler(
        IMediator mediator,
        ILogger<ApplicationSubmittedEventHandler> logger,
        IRecruitApiClient<RecruitApiConfiguration> apiClient,
        EmailEnvironmentHelper emailHelper,
        INotificationService notificationService) : INotificationHandler<ApplicationSubmittedEvent>
    {
        public async Task Handle(ApplicationSubmittedEvent @event, CancellationToken cancellationToken)
        {
            // Get the vacancy from the vacancy reference
            var result = await mediator.Send(new GetVacancyByReferenceQuery(@event.VacancyReference), cancellationToken);
            if (result == GetVacancyByReferenceQueryResult.None)
            {
                logger.LogError("ApplicationSubmittedEventHandler: Vacancy not found '{VacancyReference}'", @event.VacancyReference);
                return;
            }
            var vacancy = result.Vacancy;

            // Get the employer's unique ref details off the vacancy
            if (vacancy.AccountId is not long employerAccountId)
            {
                logger.LogError("ApplicationSubmittedEventHandler: Vacancy does not have an associated employer account '{VacancyReference}'", @event.VacancyReference);
                return;
            }

            // Get the users
            var users = await apiClient.GetAll<RecruitUserApiResponse>(
                new GetEmployerRecruitUserNotificationPreferencesApiRequest(employerAccountId,
                    NotificationTypes.ApplicationSubmitted));

            // Get the users' notification preferences
            // TODO: Filter the users based on their preferences, for immediate notifications
            var usersToNotify = users?
                .Where(x => x.NotificationPreferences.EventPreferences.Any(p =>
                    p.Event == NotificationTypes.ApplicationSubmitted &&
                    (p.Scope == NotificationScope.OrganisationVacancies || p.Scope == NotificationScope.UserSubmittedVacancies)))
                .ToList();

            // Send the email to the employer
            throw new NotImplementedException();
        }
    }
}

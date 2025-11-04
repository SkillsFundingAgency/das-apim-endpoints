using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.Recruit.Events;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Handlers;

public class VacancySubmittedEventHandler(
    ILogger<VacancySubmittedEventHandler> logger,
    IRecruitApiClient<RecruitApiConfiguration> apiClient,
    INotificationService notificationService): INotificationHandler<VacancySubmittedEvent>
{
    public async Task Handle(VacancySubmittedEvent @event, CancellationToken cancellationToken)
    {
        var response = await apiClient.PostWithResponseCode<PostCreateVacancyNotificationsResponse>(
            new PostCreateVacancyNotificationsRequest(@event.VacancyId));

        if (!response.StatusCode.IsSuccessStatusCode())
        {
            logger.LogError("Failed to create notifications for vacancy id '{VacancyId}' with error '{ErrorContent}'", @event.VacancyId, response.ErrorContent);
            return;
        }

        var sendEmailTasks = response.Body
            .Select(x => notificationService.Send(new SendEmailCommand(x.TemplateId.ToString(), x.RecipientAddress, x.Tokens)))
            .ToList();
        await Task.WhenAll(sendEmailTasks);
    }
}
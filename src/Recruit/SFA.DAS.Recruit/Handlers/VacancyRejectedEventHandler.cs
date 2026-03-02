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

public class VacancyRejectedEventHandler(
    ILogger<VacancyRejectedEventHandler> logger,
    IRecruitApiClient<RecruitApiConfiguration> apiClient,
    INotificationService notificationService): INotificationHandler<VacancyRejectedEvent>
{
    public async Task Handle(VacancyRejectedEvent @event, CancellationToken cancellationToken)
    {
        var response = await apiClient.PostWithResponseCode<PostCreateVacancyNotificationsResponse>(
            new PostCreateVacancyNotificationsRequest(@event.Id));

        if (!response.StatusCode.IsSuccessStatusCode())
        {
            logger.LogError("Failed to create notifications for vacancy id '{Id}' with error '{ErrorContent}'", @event.Id, response.ErrorContent);
            return;
        }

        var sendEmailTasks = response.Body
            .Select(x => notificationService.Send(new SendEmailCommand(x.TemplateId.ToString(), x.RecipientAddress, x.Tokens)))
            .ToList();
        await Task.WhenAll(sendEmailTasks);
    }
}
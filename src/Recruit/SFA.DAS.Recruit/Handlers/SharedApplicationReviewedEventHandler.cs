using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.Recruit.Events;
using SFA.DAS.Recruit.InnerApi.Models;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Handlers;
public class SharedApplicationReviewedEventHandler(
    ILogger<SharedApplicationReviewedEventHandler> logger,
    IRecruitApiClient<RecruitApiConfiguration> apiClient,
    INotificationService notificationService) : INotificationHandler<SharedApplicationReviewedEvent>
{
    public async Task Handle(SharedApplicationReviewedEvent @event, CancellationToken cancellationToken)
    {
        var response = await apiClient.PostWithResponseCode<List<NotificationEmailDto>>(
            new PostCreateApplicationReviewNotificationsRequest(@event.ApplicationReviewId));

        if (!response.StatusCode.IsSuccessStatusCode())
        {
            logger.LogError("Failed to create notifications for application review id '{ApplicationReviewId}' with error '{ErrorContent}'", @event.ApplicationReviewId, response.ErrorContent);
            return;
        }

        var sendEmailTasks = response.Body
            .Select(x => notificationService.Send(new SendEmailCommand(x.TemplateId.ToString(), x.RecipientAddress, x.Tokens)))
            .ToList();

        await Task.WhenAll(sendEmailTasks);
    }
}
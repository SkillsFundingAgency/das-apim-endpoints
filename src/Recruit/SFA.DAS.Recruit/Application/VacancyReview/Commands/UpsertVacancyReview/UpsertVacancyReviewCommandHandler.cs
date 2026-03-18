using MediatR;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Extensions;

using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Infrastructure;

namespace SFA.DAS.Recruit.Application.VacancyReview.Commands.UpsertVacancyReview;

public class UpsertVacancyReviewCommandHandler(
    ILogger<UpsertVacancyReviewCommandHandler> logger,
    IRecruitApiClient<RecruitApiConfiguration> apiClient,
    INotificationService notificationService) : IRequestHandler<UpsertVacancyReviewCommand>
{
    public async Task Handle(UpsertVacancyReviewCommand request, CancellationToken cancellationToken)
    {
        await apiClient.PutWithResponseCode<NullResponse>(new PutCreateVacancyReviewRequest(request.Id, request.VacancyReview));

        if (request.VacancyReview.Status.Equals("New", StringComparison.CurrentCultureIgnoreCase))
        {
            var response = await apiClient.PostWithResponseCode<PostCreateVacancyNotificationsResponse>(
                new PostCreateVacancyNotificationsRequest(request.VacancyReview.VacancyId));

            if (!response.StatusCode.IsSuccessStatusCode())
            {
                logger.LogError("Failed to create notifications for vacancy id '{Id}' with error '{ErrorContent}'", request.VacancyReview.VacancyId, response.ErrorContent);
                return;
            }

            var sendEmailTasks = response.Body
                .Select(x => notificationService.Send(new SendEmailCommand(x.TemplateId.ToString(), x.RecipientAddress, x.Tokens)))
                .ToList();
            await Task.WhenAll(sendEmailTasks);
        }
    }
}
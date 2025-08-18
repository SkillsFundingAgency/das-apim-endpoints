using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.Domain.EmailTemplates;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Application.VacancyReview.Commands.UpsertVacancyReview;

public class UpsertVacancyReviewCommandHandler(IRecruitApiClient<RecruitApiConfiguration> apiClient, INotificationService notificationService, EmailEnvironmentHelper helper) : IRequestHandler<UpsertVacancyReviewCommand>
{
    public async Task Handle(UpsertVacancyReviewCommand request, CancellationToken cancellationToken)
    {
        await apiClient.PutWithResponseCode<NullResponse>(
            new PutCreateVacancyReviewRequest(request.Id, request.VacancyReview));
        
        if (!string.IsNullOrEmpty(request.VacancyReview.ManualOutcome) 
            && request.VacancyReview.ManualOutcome.Equals("Approved", StringComparison.CurrentCultureIgnoreCase))
        {
            var users = await apiClient.GetAll<RecruitUserApiResponse>(
                new GetEmployerRecruitUserNotificationPreferencesApiRequest(request.VacancyReview.AccountId));

            var usersToNotify = users.Where(user => user.NotificationPreferences.EventPreferences.Any(c =>
                c.Event.Equals("VacancyApprovedOrRejectedByDfE", StringComparison.CurrentCultureIgnoreCase) &&
                c.Frequency.Equals("Default", StringComparison.CurrentCultureIgnoreCase))).ToList();

            var emailTasks = usersToNotify
                .Select(apiResponse => new VacancyReviewResponseEmailTemplate(
                    helper.VacancyReviewApprovedTemplateId,
                    apiResponse.Email, 
                    request.VacancyReview.VacancyTitle, 
                    apiResponse.Name,
                    request.VacancyReview.EmployerName,
                    string.Format(helper.LiveVacancyUrl, request.VacancyReview.VacancyReference.ToString()),
                    string.Format(helper.NotificationsSettingsEmployerUrl, request.VacancyReview.HashedAccountId),
                    request.VacancyReview.VacancyReference.ToString()))
                .Select(email => new SendEmailCommand(email.TemplateId, email.RecipientAddress, email.Tokens))
                .Select(notificationService.Send).ToList();
            
            await Task.WhenAll(emailTasks);
        }
    }
}
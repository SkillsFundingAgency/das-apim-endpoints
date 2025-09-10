using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.Domain.EmailTemplates;
using SFA.DAS.Recruit.Enums;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.Recruit.Application.VacancyReview.Commands.UpsertVacancyReview;

public class UpsertVacancyReviewCommandHandler(IRecruitApiClient<RecruitApiConfiguration> apiClient, INotificationService notificationService, EmailEnvironmentHelper helper) : IRequestHandler<UpsertVacancyReviewCommand>
{
    public async Task Handle(UpsertVacancyReviewCommand request, CancellationToken cancellationToken)
    {
        await apiClient.PutWithResponseCode<NullResponse>(
            new PutCreateVacancyReviewRequest(request.Id, request.VacancyReview));
        
        if (!string.IsNullOrEmpty(request.VacancyReview.ManualOutcome) 
            && (request.VacancyReview.ManualOutcome.Equals("Approved", StringComparison.CurrentCultureIgnoreCase) 
                || request.VacancyReview.ManualOutcome.Equals("Referred", StringComparison.CurrentCultureIgnoreCase )))
        {
            var employerUsersTask = request.VacancyReview.OwnerType.Equals("Employer", StringComparison.CurrentCultureIgnoreCase) 
                ? apiClient.GetAll<RecruitUserApiResponse>(new GetEmployerRecruitUserNotificationPreferencesApiRequest(request.VacancyReview.AccountId, NotificationTypes.VacancyApprovedOrRejected))
                : Task.FromResult(new List<RecruitUserApiResponse>().AsEnumerable());
            var providerUsersTask = request.VacancyReview.OwnerType.Equals("Provider", StringComparison.CurrentCultureIgnoreCase) 
                ? apiClient.GetAll<RecruitUserApiResponse>(new GetProviderRecruitUserNotificationPreferencesApiRequest(request.VacancyReview.Ukprn, NotificationTypes.VacancyApprovedOrRejected)) 
                : Task.FromResult(new List<RecruitUserApiResponse>().AsEnumerable());

            await Task.WhenAll(employerUsersTask, providerUsersTask);
            var employerUsers = await employerUsersTask;
            var providerUsers = await providerUsersTask;
            
            var usersToNotify = employerUsers.Where(user => user.NotificationPreferences.EventPreferences.Any(c =>
                c.Frequency.Equals(NotificationFrequency.Immediately) || c.Frequency.Equals(NotificationFrequency.NotSet))).ToList();
            
            var providerUsersToNotify = providerUsers.Where(user => user.NotificationPreferences.EventPreferences.Any(c =>
                c.Frequency.Equals(NotificationFrequency.Immediately) || c.Frequency.Equals(NotificationFrequency.NotSet))).ToList();

            var emailTasks = usersToNotify
                .Select(apiResponse => VacancyReviewResponseEmailTemplate(request, apiResponse, request.VacancyReview.ManualOutcome, true))
                .Where(c=>c != null)
                .Select(email => new SendEmailCommand(email.TemplateId, email.RecipientAddress, email.Tokens))
                .Select(notificationService.Send).ToList();
            
            emailTasks.AddRange(providerUsersToNotify
                .Select(apiResponse => VacancyReviewResponseEmailTemplate(request, apiResponse, request.VacancyReview.ManualOutcome, false))
                .Where(c=>c != null)
                .Select(email => new SendEmailCommand(email.TemplateId, email.RecipientAddress, email.Tokens))
                .Select(notificationService.Send).ToList());
            
            await Task.WhenAll(emailTasks);
        }
    }

    private EmailTemplateArguments VacancyReviewResponseEmailTemplate(UpsertVacancyReviewCommand request, RecruitUserApiResponse apiResponse, string outcome, bool isEmployer)
    {
        if (outcome.Equals("Approved", StringComparison.CurrentCultureIgnoreCase))
        {
            return new VacancyReviewResponseEmailTemplate(
                isEmployer ? helper.VacancyReviewApprovedEmployerTemplateId 
                    : helper.VacancyReviewApprovedProviderTemplateId,
                apiResponse.Email, 
                request.VacancyReview.VacancyTitle, 
                apiResponse.FirstName,
                request.VacancyReview.EmployerName,
                string.Format(helper.LiveVacancyUrl, request.VacancyReview.VacancyReference.ToString()),
                isEmployer ? string.Format(helper.NotificationsSettingsEmployerUrl, request.VacancyReview.HashedAccountId):
                    string.Format(helper.NotificationsSettingsProviderUrl, request.VacancyReview.Ukprn),
                request.VacancyReview.VacancyReference.ToString(),
                request.VacancyReview.EmployerLocationOption == AvailableWhere.AcrossEngland ? "Recruiting nationally" 
                    : EmailTemplateAddressExtension.GetEmploymentLocationCityNames(request.VacancyReview.EmployerLocations));    
        }
        if (outcome.Equals("Referred", StringComparison.CurrentCultureIgnoreCase))
        {
            return new VacancyReviewRejectedResponseEmailTemplate(
                isEmployer ? helper.VacancyReviewEmployerRejectedByDfeTemplateId
                    : helper.VacancyReviewProviderRejectedByDfeTemplateId,
                apiResponse.Email, 
                request.VacancyReview.VacancyTitle, 
                apiResponse.FirstName,
                request.VacancyReview.EmployerName,
                isEmployer ? string.Format(helper.ReviewVacancyReviewInRecruitEmployerUrl, request.VacancyReview.HashedAccountId, request.VacancyReview.VacancyId.ToString())
                    : string.Format(helper.ReviewVacancyReviewInRecruitProviderUrl, request.VacancyReview.Ukprn, request.VacancyReview.VacancyId.ToString()),
                isEmployer ? string.Format(helper.NotificationsSettingsEmployerUrl, request.VacancyReview.HashedAccountId):
                    string.Format(helper.NotificationsSettingsProviderUrl, request.VacancyReview.Ukprn),
                request.VacancyReview.VacancyReference.ToString(),
                request.VacancyReview.EmployerLocationOption == AvailableWhere.AcrossEngland ? "Recruiting nationally" 
                    : EmailTemplateAddressExtension.GetEmploymentLocationCityNames(request.VacancyReview.EmployerLocations));   
        }

        return null;
    }
}
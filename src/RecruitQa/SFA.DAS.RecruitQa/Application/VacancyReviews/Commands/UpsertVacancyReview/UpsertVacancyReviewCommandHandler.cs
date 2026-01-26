using MediatR;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.RecruitQa.Domain;
using SFA.DAS.RecruitQa.Domain.EmailTemplates;
using SFA.DAS.RecruitQa.InnerApi;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.RecruitQa.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.RecruitQa.Application.VacancyReviews.Commands.UpsertVacancyReview;

public class UpsertVacancyReviewCommandHandler(
    IRecruitApiClient<RecruitApiConfiguration> apiClient,
    INotificationService notificationService,
    ICourseService courseService,
    EmailEnvironmentHelper helper) : IRequestHandler<UpsertVacancyReviewCommand>
{
    public async Task Handle(UpsertVacancyReviewCommand request, CancellationToken cancellationToken)
    {
        await apiClient.PutWithResponseCode<NullResponse>(new PutCreateVacancyReviewRequest(request.Id, request.VacancyReview));

        if (!string.IsNullOrEmpty(request.VacancyReview.ManualOutcome) 
            && (request.VacancyReview.ManualOutcome.Equals("Approved", StringComparison.CurrentCultureIgnoreCase) 
                || request.VacancyReview.ManualOutcome.Equals("Referred", StringComparison.CurrentCultureIgnoreCase)))
        {
            var employerUsersTask = request.VacancyReview.OwnerType.Equals("Employer", StringComparison.CurrentCultureIgnoreCase) 
                ? apiClient.GetAll<RecruitUserApiResponse>(new GetEmployerRecruitUserNotificationPreferencesApiRequest(request.VacancyReview.AccountId, NotificationTypes.VacancyApprovedOrRejected))
                : Task.FromResult(new List<RecruitUserApiResponse>().AsEnumerable());
            
            var providerUsersTask = apiClient.GetAll<RecruitUserApiResponse>(new GetProviderRecruitUserNotificationPreferencesApiRequest(request.VacancyReview.Ukprn, NotificationTypes.VacancyApprovedOrRejected));
            var providerUsersAttachedTask = apiClient.GetAll<RecruitUserApiResponse>(new GetProviderRecruitUserNotificationPreferencesApiRequest(request.VacancyReview.Ukprn, NotificationTypes.ProviderAttachedToVacancy));
            
            await Task.WhenAll(employerUsersTask, providerUsersTask, providerUsersAttachedTask);
        
            var employerUsers = await employerUsersTask;
            var providerUsersTaskResult = await providerUsersTask;
            var providerUsers = providerUsersTaskResult.ToList();
            var providerUsersAttached = await providerUsersAttachedTask;
        
            var usersToNotify = employerUsers
                .Where(user => user.NotificationPreferences.EventPreferences.Any(c =>
                    c.Frequency.Equals(NotificationFrequency.Immediately))).ToList();
        
            var providerUsersToNotifyOnVacancyApprovedOrRejected = providerUsers
                .Where(user => user.NotificationPreferences.EventPreferences.Any(c => c.Frequency.Equals(NotificationFrequency.Immediately))).ToList();
        
            var providerUsersToNotifyOnAddingToEmployerVacancy = providerUsersAttached
                .Where(user => user.NotificationPreferences.EventPreferences.Any(c => c.Frequency.Equals(NotificationFrequency.Immediately))).ToList();
        
            var emailTasks = usersToNotify
                .Select(apiResponse => VacancyReviewResponseEmailTemplate(request, apiResponse, request.VacancyReview.ManualOutcome, true))
                .Where(c => c != null)
                .Select(email => new SendEmailCommand(email.TemplateId, email.RecipientAddress, email.Tokens))
                .Select(notificationService.Send).ToList();
        
            if (request.VacancyReview.OwnerType.Equals("Provider", StringComparison.CurrentCultureIgnoreCase))
            {
                emailTasks.AddRange(providerUsersToNotifyOnVacancyApprovedOrRejected
                    .Select(apiResponse => VacancyReviewResponseEmailTemplate(request, apiResponse, request.VacancyReview.ManualOutcome, false))
                    .Where(c => c != null)
                    .Select(email => new SendEmailCommand(email.TemplateId, email.RecipientAddress, email.Tokens))
                    .Select(notificationService.Send).ToList());    
            }
        
            if (request.VacancyReview.ManualOutcome.Equals("Approved", StringComparison.CurrentCultureIgnoreCase) &&
                request.VacancyReview.OwnerType == "Employer" &&
                providerUsersToNotifyOnAddingToEmployerVacancy.Count > 0)
            {
                var result = await apiClient.GetWithResponseCode<GetVacancyByIdResponse>(new GetVacancyByIdRequest(request.VacancyReview.VacancyId));
                if (result.Body != null)
                {
                    var vacancy = result.Body.ToDomain();
        
                    var emailResults = await Task.WhenAll(
                        providerUsersToNotifyOnAddingToEmployerVacancy
                            .Select(apiResponse => ProviderAddedToEmployerVacancy(request, apiResponse, vacancy))
                    );
        
                    var emailCommands = emailResults
                        .Where(email => email is not null)
                        .Select(email => new SendEmailCommand(email!.TemplateId, email.RecipientAddress, email.Tokens));
        
                    emailTasks.AddRange(emailCommands.Select(notificationService.Send));
                }
            }
            
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
    
    private async Task<EmailTemplateArguments> ProviderAddedToEmployerVacancy(UpsertVacancyReviewCommand request, RecruitUserApiResponse apiResponse, Vacancy vacancy)
    {
        var apprenticeCount = vacancy.NumberOfPositions > 1
            ? $"{vacancy.NumberOfPositions} apprentices"
            : "1 apprentice";
    
        var employerLocation = vacancy.EmployerLocationOption == AvailableWhere.AcrossEngland
            ? "Recruiting nationally"
            : EmailTemplateAddressExtension.GetEmploymentLocationCityNames(vacancy.EmployerLocations);
    
        var liveVacancyUrl = string.Format(helper.LiveVacancyUrl, vacancy.VacancyReference);
        var notificationSettingsUrl = string.Format(helper.NotificationsSettingsProviderUrl, request.VacancyReview.Ukprn);
    
        var trainingProgramme = await GetTrainingProgrammeById(vacancy.ProgrammeId);
    
        return new ProviderAddedToEmployerVacancyEmailTemplate(templateId: helper.ProviderAddedToEmployerVacancy,
            apiResponse.Email,
            firstName: apiResponse.FirstName,
            vacancy.Title,
            vacancyReference: vacancy.VacancyReference.ToString(),
            employerName: vacancy.EmployerName,
            request.VacancyReview.SubmittedByUserEmail,
            employerLocation,
            liveVacancyUrl,
            trainingProgramme?.Title,
            apprenticeCount,
            vacancy.StartDate.ToDayMonthYearString() ?? string.Empty,
            vacancy.Wage.GetDuration(),
            notificationSettingsUrl);
    }
    
    private async Task<TrainingProgramme?> GetTrainingProgrammeById(string programmeId)
    {
        var standards = await courseService.GetActiveStandards<GetStandardsListResponse>("ActiveStandards");
        var allTrainingProgrammes = standards.Standards?
            .Select(item => (TrainingProgramme)item)
            .ToList() ?? [];
    
        return allTrainingProgrammes.FirstOrDefault(c => c.Id.Equals(programmeId, StringComparison.CurrentCultureIgnoreCase));
    }
}
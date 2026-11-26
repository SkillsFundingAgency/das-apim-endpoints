using MediatR;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Domain.EmailTemplates;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitV2Api.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitV2Api.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;
using SFA.DAS.Apim.Shared.Models;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.SubmitApplication;

public class SubmitApplicationCommandHandler(
    IRecruitApiClient<RecruitApiV2Configuration> recruitApiV2Client, 
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient,
    IVacancyService vacancyService,
    INotificationService notificationService,
    IMetrics metrics,
    EmailEnvironmentHelper helper,
    ILogger<SubmitApplicationCommandHandler> logger)
    : IRequestHandler<SubmitApplicationCommand, bool>
{
    public async Task<bool> Handle(SubmitApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await GetApplicationAsync(request);
        if (application is null) return false;

        var vacancy = await GetVacancyAsync(application);
        if (vacancy is null) return false;

        await CreateApplicationReviewAsync(application, vacancy);

        await Task.WhenAll(
            SendCandidateConfirmationEmailAsync(application, vacancy),
            MarkApplicationSubmittedAsync(request.ApplicationId, request.CandidateId));

        if (vacancy.VacancySource == VacancyDataSource.Raa)
            metrics.IncreaseVacancySubmitted(application.VacancyReference);

        await SendRecruitNotificationsAsync(request.ApplicationId);

        return true;
    }

    // Fetch Application
    private async Task<GetApplicationApiResponse?> GetApplicationAsync(SubmitApplicationCommand request)
    {
        var app = await candidateApiClient.Get<GetApplicationApiResponse>(
            new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, true));

        if (app is null || app.Status == ApplicationStatus.Submitted)
            return null;

        return app;
    }

    // Fetch Vacancy
    private async Task<GetApprenticeshipVacancyItemResponse?> GetVacancyAsync(GetApplicationApiResponse application)
    {
        var vacancy = await vacancyService.GetVacancy(application.VacancyReference);
        return vacancy as GetApprenticeshipVacancyItemResponse;
    }

    // Create application review in Recruit
    private async Task CreateApplicationReviewAsync(GetApplicationApiResponse app, GetApprenticeshipVacancyItemResponse vacancy)
    {
        int.TryParse(vacancy.Ukprn, out var ukprn);

        var data = new CreateApplicationReviewRequestData(
            vacancy.AccountId!.Value,
            vacancy.AccountLegalEntityId!.Value,
            app.Id,
            app.CandidateId,
            ukprn,
            vacancy.VacancyReference.ConvertVacancyReferenceToLong(),
            vacancy.Title,
            vacancy.AdditionalQuestion1,
            vacancy.AdditionalQuestion2,
            DateTime.UtcNow);

        var response = await recruitApiV2Client.PutWithResponseCode<NullResponse>(
            new CreateApplicationReviewRequest(app.Id, data));

        if (!response.StatusCode.IsSuccessStatusCode())
        {
            logger.LogError("Failed to create application review in Recruit v2 for app '{Id}' - {Error}",
                app.Id, response.ErrorContent);
            throw new ApplicationException($"Failed to create application review in Recruit v2 for app '{app.Id}'");
        }
    }

    // Send candidate confirmation email
    private async Task SendCandidateConfirmationEmailAsync(GetApplicationApiResponse app, GetApprenticeshipVacancyItemResponse vacancy)
    {
        var email = new SubmitApplicationEmail(
            helper.SubmitApplicationEmailTemplateId,
            app.Candidate.Email,
            app.Candidate.FirstName,
            vacancy.Title,
            vacancy.EmployerName,
            vacancyService.GetVacancyWorkLocation(vacancy, true),
            helper.CandidateApplicationUrl);

        await notificationService.Send(new SendEmailCommand(email.TemplateId, email.RecipientAddress, email.Tokens));
    }

    // Update application status
    private async Task MarkApplicationSubmittedAsync(Guid applicationId, Guid candidateId)
    {
        var patch = new JsonPatchDocument<Domain.Models.Application>();
        patch.Replace(x => x.Status, ApplicationStatus.Submitted);

        var patchApplicationApiRequest = new PatchApplicationApiRequest(applicationId, candidateId, patch);
        
        var patchApplicationPolicy = PatchApplicationPolicy(patchApplicationApiRequest, applicationId);

        var result = await patchApplicationPolicy.ExecuteAsync(() => candidateApiClient.PatchWithResponseCode(patchApplicationApiRequest));
        if (!result.StatusCode.IsSuccessStatusCode())
        {
            logger.LogError("Failed to update application status for app '{Id}' - {Error}", applicationId, result.ErrorContent);
            throw new ApplicationException($"Failed to update application status for app '{applicationId}'");
        }
    }

    // Send Recruit notification emails
    private async Task SendRecruitNotificationsAsync(Guid applicationId)
    {
        var response = await recruitApiV2Client.PostWithResponseCode<PostCreateApplicationReviewNotificationsResponse>(
            new PostCreateApplicationReviewNotificationsRequest(applicationId));

        if (!response.StatusCode.IsSuccessStatusCode())
        {
            logger.LogError("Failed to create application review notifications for app '{Id}' - {Error}",
                applicationId, response.ErrorContent);
            return;
        }

        var sendEmailTasks = response.Body
            .Select(x => notificationService.Send(
                new SendEmailCommand(x.TemplateId.ToString(), x.RecipientAddress, x.Tokens)))
            .ToArray();

        await Task.WhenAll(sendEmailTasks);
    }
    
    
    private AsyncRetryPolicy<ApiResponse<string>> PatchApplicationPolicy(PatchApplicationApiRequest request, Guid applicationId)
    {
        return Policy
            .HandleResult<ApiResponse<string>>(r => r.StatusCode != HttpStatusCode.OK)
            .WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(200), 
                (response, _, retryCount, _) =>
                {
                    logger.LogInformation("SubmitApplicationCommandHandler: Unable to update {ApplicationId}. Retry {RetryCount} due to {StatusCode} response", applicationId, retryCount, response.Result.StatusCode);
                });
    }
}
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
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
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
        var application =
            await candidateApiClient.Get<GetApplicationApiResponse>(
                new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, true));

        if (application == null || application.Status == ApplicationStatus.Submitted)
            return false;

        // Check if the vacancy is still Live and not closed.
        if (await vacancyService.GetVacancy(application.VacancyReference) is not GetApprenticeshipVacancyItemResponse vacancy)
            return false;

        if (!int.TryParse(vacancy.Ukprn, out var ukprn))
            ukprn = 0;

        var reviewRequestData = new CreateApplicationReviewRequestData(
            vacancy.AccountId!.Value,
            vacancy.AccountLegalEntityId!.Value,
            application.Id,
            application.CandidateId,
            ukprn,
            vacancy.VacancyReference.ConvertVacancyReferenceToLong(),
            vacancy.Title,
            vacancy.AdditionalQuestion1,
            vacancy.AdditionalQuestion2,
            DateTime.UtcNow);

        var createReviewTask = recruitApiV2Client.PutWithResponseCode<NullResponse>(
            new CreateApplicationReviewRequest(application.Id, reviewRequestData));

        // Send candidate confirmation email
        var email = new SubmitApplicationEmail(
            helper.SubmitApplicationEmailTemplateId,
            application.Candidate.Email,
            application.Candidate.FirstName,
            vacancy.Title,
            vacancy.EmployerName,
            vacancyService.GetVacancyWorkLocation(vacancy, true),
            helper.CandidateApplicationUrl);

        var candidateEmailTask = notificationService.Send(
            new SendEmailCommand(email.TemplateId, email.RecipientAddress, email.Tokens));

        // Mark application as submitted
        var jsonPatch = new JsonPatchDocument<Domain.Models.Application>();
        jsonPatch.Replace(x => x.Status, ApplicationStatus.Submitted);

        var patchTask = candidateApiClient.PatchWithResponseCode(new PatchApplicationApiRequest(request.ApplicationId, request.CandidateId, jsonPatch));

        // Increase metrics (no await needed)
        if (vacancy.VacancySource == VacancyDataSource.Raa)
            metrics.IncreaseVacancySubmitted(application.VacancyReference);

        // Wait for the core tasks together
        await Task.WhenAll(createReviewTask, candidateEmailTask, patchTask);

        // Recruit notifications (dependent on review creation)
        var recruitNotificationResponse = await recruitApiV2Client.PostWithResponseCode<PostCreateApplicationReviewNotificationsResponse>(
            new PostCreateApplicationReviewNotificationsRequest(request.ApplicationId));

        if (!recruitNotificationResponse.StatusCode.IsSuccessStatusCode())
        {
            logger.LogError("Failed to create application review notifications for application id '{Id}' with error '{ErrorContent}'",
                request.ApplicationId,
                recruitNotificationResponse.ErrorContent);
            return true;
        }

        // Send recruit notification emails concurrently
        var recruitEmailTasks = recruitNotificationResponse.Body
            .Select(n => notificationService.Send(
                new SendEmailCommand(n.TemplateId.ToString(), n.RecipientAddress, n.Tokens)))
            .ToArray();

        await Task.WhenAll(recruitEmailTasks);

        return true;
    }
}
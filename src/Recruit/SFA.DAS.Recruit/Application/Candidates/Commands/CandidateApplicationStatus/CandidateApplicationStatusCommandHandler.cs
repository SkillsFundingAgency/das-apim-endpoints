using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.Domain.EmailTemplates;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Recruit.Enums;

namespace SFA.DAS.Recruit.Application.Candidates.Commands.CandidateApplicationStatus;

public class CandidateApplicationStatusCommandHandler(
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient,
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
    INotificationService notificationService,
    EmailEnvironmentHelper emailEnvironmentHelper)
    : IRequestHandler<CandidateApplicationStatusCommand, Unit>
{
    public async Task<Unit> Handle(CandidateApplicationStatusCommand request, CancellationToken cancellationToken)
    {
        ApiResponse<GetCandidateApiResponse> candidate = null;
        if (request.ApplicationId == Guid.Empty)
        {
            candidate =
                await candidateApiClient.GetWithResponseCode<GetCandidateApiResponse>(new GetCandidateByMigratedCandidateIdApiRequest(request.CandidateId));
            if (candidate.StatusCode == HttpStatusCode.NotFound)
            {
                return new Unit();
            }
            var applicationRequest =
                new GetApplicationByReferenceApiRequest(candidate.Body.Id, request.VacancyReference);
            var application = await
                candidateApiClient.GetWithResponseCode<GetApplicationByReferenceApiResponse>(applicationRequest);
            request.ApplicationId = application.Body.Id;
            request.CandidateId = application.Body.CandidateId;
        }

        candidate ??=
            await candidateApiClient.GetWithResponseCode<GetCandidateApiResponse>(
                new GetCandidateByIdApiRequest(request.CandidateId));
        
        var jsonPatchDocument = new JsonPatchDocument<InnerApi.Requests.Application>();
        var applicationStatus = request.Outcome.Equals("Successful", StringComparison.CurrentCultureIgnoreCase)
            ? ApplicationStatus.Successful
            : ApplicationStatus.UnSuccessful;
        jsonPatchDocument.Replace(x => x.ResponseNotes, request.Feedback);
        jsonPatchDocument.Replace(x => x.Status, applicationStatus);
        var patchRequest = new PatchApplicationApiRequest(request.ApplicationId, request.CandidateId, jsonPatchDocument);

        var jsonPatchApplicationReviewDocument = new JsonPatchDocument<ApplicationReviewStatusData>();
        jsonPatchApplicationReviewDocument.Replace(x => x.CandidateFeedback, request.Feedback);
        jsonPatchApplicationReviewDocument.Replace(x => x.Status, Enum.GetName(applicationStatus));
        jsonPatchApplicationReviewDocument.Replace(x => x.StatusUpdatedDate, DateTime.UtcNow);
        var patchApplicationReviewApiRequest = new PatchRecruitApplicationReviewStatusApiRequest(request.ApplicationId, jsonPatchApplicationReviewDocument);
        
        SendEmailCommand sendEmailCommand;
        if (applicationStatus == ApplicationStatus.Successful)
        {
            var email = new ApplicationResponseSuccessEmailTemplate(
                emailEnvironmentHelper.SuccessfulApplicationEmailTemplateId, 
                candidate.Body.Email,
                candidate.Body.FirstName, request.VacancyTitle, request.VacancyEmployerName,
                GetLocation(request.VacancyLocation));
            sendEmailCommand = new SendEmailCommand(email.TemplateId, email.RecipientAddress, email.Tokens);
        }
        else
        {
            var unsuccessfulEmail = new ApplicationResponseUnsuccessfulEmailTemplate(
                emailEnvironmentHelper.UnsuccessfulApplicationEmailTemplateId,
                candidate.Body.Email,
                candidate.Body.FirstName, request.VacancyTitle, request.VacancyEmployerName,
                GetLocation(request.VacancyLocation),
                request.Feedback, emailEnvironmentHelper.CandidateApplicationUrl);
            sendEmailCommand = new SendEmailCommand(unsuccessfulEmail.TemplateId, unsuccessfulEmail.RecipientAddress, unsuccessfulEmail.Tokens);
        }
        
        await Task.WhenAll(
            notificationService.Send(sendEmailCommand), 
            candidateApiClient.PatchWithResponseCode(patchRequest),
            recruitApiClient.PatchWithResponseCode(patchApplicationReviewApiRequest));
        return new Unit();
    }

    /// <summary>
    /// Gets the location of the vacancy from the request.
    /// If the VacancyLocation is not provided, it returns "Unknown".
    /// </summary>
    /// <param name="vacancyLocation"></param>
    /// <returns>The location of the vacancy or "Unknown" if not provided. This ensures that the email tokens always have a meaningful value, avoiding null or empty strings.</returns>
    private static string GetLocation(string vacancyLocation)
    {
        return !string.IsNullOrWhiteSpace(vacancyLocation) ? vacancyLocation : "Unknown";
    }
}
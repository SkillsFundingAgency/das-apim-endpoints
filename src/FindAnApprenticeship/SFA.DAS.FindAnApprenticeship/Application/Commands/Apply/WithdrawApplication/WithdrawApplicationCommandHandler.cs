using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.FindAnApprenticeship.Domain.EmailTemplates;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitV2Api.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.WithdrawApplication;

public class WithdrawApplicationCommandHandler(
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient, 
    IRecruitApiClient<RecruitApiV2Configuration> recruitApiV2Client,
    INotificationService notificationService,
    IVacancyService vacancyService,
    EmailEnvironmentHelper emailEnvironmentHelper) : IRequestHandler<WithdrawApplicationCommand, bool>
{
    public async Task<bool> Handle(WithdrawApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await candidateApiClient.Get<GetApplicationApiResponse>(
            new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, true));

        if (application is not { Status: ApplicationStatus.Submitted })
        {
            return false;
        }

        //need to get the application from recruit v2 for the Id. 
        var applicationReview =
            await recruitApiV2Client.GetWithResponseCode<ApplicationReview>(
                new GetApplicationReviewByApplicationIdRequest(request.ApplicationId));

        var patchTask = Task.FromResult(new ApiResponse<string>("", HttpStatusCode.Accepted, ""));

        if (applicationReview.StatusCode == HttpStatusCode.OK)
        {
            var jsonPatchApplicationReviewDocument = new JsonPatchDocument<ApplicationReview>();
            jsonPatchApplicationReviewDocument.Replace(x => x.WithdrawnDate, DateTime.UtcNow);
            var patchApplicationReviewApiRequest = new PatchRecruitApplicationReviewApiRequest(applicationReview.Body.Id, jsonPatchApplicationReviewDocument);

            patchTask = recruitApiV2Client.PatchWithResponseCode(patchApplicationReviewApiRequest);    
        }
        

        var jsonPatchDocument = new JsonPatchDocument<Domain.Models.Application>();
        jsonPatchDocument.Replace(x => x.Status, ApplicationStatus.Withdrawn);

        var patchRequest = new PatchApplicationApiRequest(request.ApplicationId, request.CandidateId, jsonPatchDocument);

        var vacancy = await vacancyService.GetVacancy(application.VacancyReference);
        var withDrawnApplicationEmail = vacancy is GetApprenticeshipVacancyItemResponse apprenticeshipVacancy
            ? GetWithdrawApplicationEmail(application, apprenticeshipVacancy)
            : GetWithdrawApplicationEmail(application, await vacancyService.GetClosedVacancy(application.VacancyReference) as GetClosedVacancyResponse);

        await Task.WhenAll(
            candidateApiClient.PatchWithResponseCode(patchRequest),
            notificationService.Send(new SendEmailCommand(withDrawnApplicationEmail.TemplateId, withDrawnApplicationEmail.RecipientAddress, withDrawnApplicationEmail.Tokens)),
            patchTask
        );

        return true;
    }

    private WithdrawApplicationEmail GetWithdrawApplicationEmail(GetApplicationApiResponse application, GetClosedVacancyResponse vacancyResponse)
    {
        return new WithdrawApplicationEmail(
            emailEnvironmentHelper.WithdrawApplicationEmailTemplateId,
            application.Candidate.Email,
            application.Candidate.FirstName,
            vacancyResponse.Title,
            vacancyResponse.EmployerName,
            vacancyService.GetVacancyWorkLocation(vacancyResponse, true));
    }

    private WithdrawApplicationEmail GetWithdrawApplicationEmail(GetApplicationApiResponse application, GetApprenticeshipVacancyItemResponse vacancyResponse)
    {
        return new WithdrawApplicationEmail(
            emailEnvironmentHelper.WithdrawApplicationEmailTemplateId,
            application.Candidate.Email,
            application.Candidate.FirstName,
            vacancyResponse.Title,
            vacancyResponse.EmployerName,
            vacancyService.GetVacancyWorkLocation(vacancyResponse, true));
    }
}
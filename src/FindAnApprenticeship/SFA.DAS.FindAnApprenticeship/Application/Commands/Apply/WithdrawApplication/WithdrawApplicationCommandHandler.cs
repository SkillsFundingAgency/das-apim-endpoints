using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.FindAnApprenticeship.Domain.EmailTemplates;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitV2Api.Requests;
using SFA.DAS.SharedOuterApi.Extensions;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.WithdrawApplication;

public class WithdrawApplicationCommandHandler(
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient, 
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
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

        var response = await recruitApiClient.PostWithResponseCode<NullResponse>(
            new PostWithdrawApplicationRequest(request.CandidateId, Convert.ToInt64(application.VacancyReference.TrimVacancyReference())), false);

        if (response.StatusCode != HttpStatusCode.NoContent)
        {
            return false;
        }
        
        var jsonPatchApplicationReviewDocument = new JsonPatchDocument<ApplicationReview>();
        jsonPatchApplicationReviewDocument.Replace(x => x.WithdrawnDate, DateTime.UtcNow);
        jsonPatchApplicationReviewDocument.Replace(x => x.Status, Enum.GetName(ApplicationStatus.Withdrawn));
        jsonPatchApplicationReviewDocument.Replace(x => x.StatusUpdatedDate, DateTime.UtcNow);
        var patchApplicationReviewApiRequest = new PatchRecruitApplicationReviewApiRequest(request.ApplicationId, jsonPatchApplicationReviewDocument);
        
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
            recruitApiV2Client.PatchWithResponseCode(patchApplicationReviewApiRequest)
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
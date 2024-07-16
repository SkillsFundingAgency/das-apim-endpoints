using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.FindAnApprenticeship.Domain.EmailTemplates;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.SubmitApplication;

public class SubmitApplicationCommandHandler(
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient, 
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient,
    IVacancyService vacancyService,
    INotificationService notificationService,
    IMetrics metrics,
    EmailEnvironmentHelper helper) 
    : IRequestHandler<SubmitApplicationCommand, bool>
{
    public async Task<bool> Handle(SubmitApplicationCommand request, CancellationToken cancellationToken)
    {
        var application =
            await candidateApiClient.Get<GetApplicationApiResponse>(
                new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, true));

        if (application == null || application.Status == "Submitted")
        {
            return false;
        }

        var response = await recruitApiClient.PostWithResponseCode<NullResponse>(
            new PostSubmitApplicationRequest(request.CandidateId, application), false);

        if (response.StatusCode != HttpStatusCode.NoContent)
        {
            return false;
        }

        var vacancy = await vacancyService.GetVacancy(application.VacancyReference) as GetApprenticeshipVacancyItemResponse;
        var email = new SubmitApplicationEmail(
            helper.SubmitApplicationEmailTemplateId,
            application.Candidate.Email,
            application.Candidate.FirstName,
            vacancy?.Title, vacancy?.EmployerName,
            vacancy?.Address.AddressLine4 ?? vacancy?.Address.AddressLine3 ?? vacancy?.Address.AddressLine2 ?? vacancy?.Address.AddressLine1,
            vacancy?.Address.Postcode,
            helper.CandidateApplicationUrl);
        await notificationService.Send(new SendEmailCommand(email.TemplateId, email.RecipientAddress, email.Tokens));
        var jsonPatchDocument = new JsonPatchDocument<Domain.Models.Application>();

        jsonPatchDocument.Replace(x => x.Status, ApplicationStatus.Submitted);

        var patchRequest = new PatchApplicationApiRequest(request.ApplicationId, request.CandidateId, jsonPatchDocument);
        await candidateApiClient.PatchWithResponseCode(patchRequest);

        // increase the count of vacancy submitted counter metrics.
        metrics.IncreaseVacancySubmitted(application.VacancyReference);

        return true;
    }
}
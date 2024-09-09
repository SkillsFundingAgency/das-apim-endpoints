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

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.WithdrawApplication;

public class WithdrawApplicationCommandHandler(
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient, 
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
    INotificationService notificationService,
    IVacancyService vacancyService,
    EmailEnvironmentHelper emailEnvironmentHelper) : IRequestHandler<WithdrawApplicationCommand, bool>
{
    public async Task<bool> Handle(WithdrawApplicationCommand request, CancellationToken cancellationToken)
    {
        var application =
            await candidateApiClient.Get<GetApplicationApiResponse>(
                new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, true));

        if (application is not { Status: ApplicationStatus.Submitted })
        {
            return false;
        }
        var response = await recruitApiClient.PostWithResponseCode<NullResponse>(
            new PostWithdrawApplicationRequest(request.CandidateId, Convert.ToInt64(application.VacancyReference.Replace("VAC",""))), false);

        if (response.StatusCode != HttpStatusCode.NoContent)
        {
            return false;
        }
        
        var jsonPatchDocument = new JsonPatchDocument<Domain.Models.Application>();
        
        jsonPatchDocument.Replace(x => x.Status, ApplicationStatus.Withdrawn);

        var vacancy = await vacancyService.GetVacancy(application.VacancyReference) as GetApprenticeshipVacancyItemResponse;
        
        var patchRequest = new PatchApplicationApiRequest(request.ApplicationId, request.CandidateId, jsonPatchDocument);
        var email = new WithdrawApplicationEmail(
            emailEnvironmentHelper.WithdrawApplicationEmailTemplateId,
            application.Candidate.Email,
            application.Candidate.FirstName,
            vacancy?.Title, vacancy?.EmployerName,
            vacancy?.Address.AddressLine4 ?? vacancy?.Address.AddressLine3 ?? vacancy?.Address.AddressLine2 ?? vacancy?.Address.AddressLine1 ?? "Unknown",
            vacancy?.Address.Postcode);
        
        await Task.WhenAll(
            candidateApiClient.PatchWithResponseCode(patchRequest)
            , notificationService.Send(new SendEmailCommand(email.TemplateId, email.RecipientAddress, email.Tokens))
            );
        
        return true;
    }
}
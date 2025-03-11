using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.Domain.EmailTemplates;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Recruit.Application.Candidates.Commands.CandidateApplicationStatus;

public class CandidateApplicationStatusCommandHandler : IRequestHandler<CandidateApplicationStatusCommand, Unit>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;
    private readonly INotificationService _notificationService;
    private readonly EmailEnvironmentHelper _emailEnvironmentHelper;

    public CandidateApplicationStatusCommandHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient, INotificationService notificationService, EmailEnvironmentHelper emailEnvironmentHelper)
    {
        _candidateApiClient = candidateApiClient;
        _notificationService = notificationService;
        _emailEnvironmentHelper = emailEnvironmentHelper;
    }
    public async Task<Unit> Handle(CandidateApplicationStatusCommand request, CancellationToken cancellationToken)
    {
        ApiResponse<GetCandidateApiResponse> candidate = null;
        if (request.ApplicationId == Guid.Empty)
        {
            candidate =
                await _candidateApiClient.GetWithResponseCode<GetCandidateApiResponse>(new GetCandidateByMigratedCandidateIdApiRequest(request.CandidateId));
            if (candidate.StatusCode == HttpStatusCode.NotFound)
            {
                return new Unit();
            }
            var applicationRequest =
                new GetApplicationByReferenceApiRequest(candidate.Body.Id, request.VacancyReference);
            var application = await
                _candidateApiClient.GetWithResponseCode<GetApplicationByReferenceApiResponse>(applicationRequest);
            request.ApplicationId = application.Body.Id;
            request.CandidateId = application.Body.CandidateId;
        }

        candidate ??=
            await _candidateApiClient.GetWithResponseCode<GetCandidateApiResponse>(
                new GetCandidateByIdApiRequest(request.CandidateId));
        
        var jsonPatchDocument = new JsonPatchDocument<InnerApi.Requests.Application>();

        var applicationStatus = request.Outcome.Equals("Successful", StringComparison.CurrentCultureIgnoreCase)
            ? ApplicationStatus.Successful
            : ApplicationStatus.UnSuccessful;
        
        jsonPatchDocument.Replace(x => x.ResponseNotes, request.Feedback);
        jsonPatchDocument.Replace(x => x.Status, applicationStatus);
        
        var patchRequest = new PatchApplicationApiRequest(request.ApplicationId, request.CandidateId, jsonPatchDocument);
        SendEmailCommand sendEmailCommand;
        if (applicationStatus == ApplicationStatus.Successful)
        {
            var email = new ApplicationResponseSuccessEmailTemplate(
                _emailEnvironmentHelper.SuccessfulApplicationEmailTemplateId, 
                candidate.Body.Email,
                candidate.Body.FirstName, request.VacancyTitle, request.VacancyEmployerName,
                GetLocation(request));
            sendEmailCommand = new SendEmailCommand(email.TemplateId, email.RecipientAddress, email.Tokens);
        }
        else
        {
            var unsuccessfulEmail = new ApplicationResponseUnsuccessfulEmailTemplate(
                _emailEnvironmentHelper.UnsuccessfulApplicationEmailTemplateId,
                candidate.Body.Email,
                candidate.Body.FirstName, request.VacancyTitle, request.VacancyEmployerName,
                GetLocation(request),
                request.Feedback, _emailEnvironmentHelper.CandidateApplicationUrl);
            sendEmailCommand = new SendEmailCommand(unsuccessfulEmail.TemplateId, unsuccessfulEmail.RecipientAddress, unsuccessfulEmail.Tokens);
        }
        
        await Task.WhenAll(_notificationService.Send(sendEmailCommand), _candidateApiClient.PatchWithResponseCode(patchRequest));
        return new Unit();
    }

    private static string GetLocation(CandidateApplicationStatusCommand request)
    {
        if (!string.IsNullOrWhiteSpace(request.VacancyLocation))
        {
            return request.VacancyLocation;
        }

        // TODO: remove once MultipleLocations is released
        return string.IsNullOrEmpty(request.VacancyCity)
            ? request.VacancyPostcode
            : string.IsNullOrEmpty(request.VacancyPostcode)
                ? request.VacancyCity
                : $"{request.VacancyCity}, {request.VacancyPostcode}";
    }
}
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Commands;

public class ProcessVacancyClosedEarlyCommandHandler(
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient, 
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient,
    EmailEnvironmentHelper helper,
    INotificationService notificationService) : IRequestHandler<ProcessVacancyClosedEarlyCommand, Unit>
{
    public async Task<Unit> Handle(ProcessVacancyClosedEarlyCommand request, CancellationToken cancellationToken)
    {
        var preference = await candidateApiClient.Get<GetPreferencesApiResponse>(new GetCandidatePreferencesRequest());
        var emailPreference = preference.Preferences
            .FirstOrDefault(c => c.PreferenceType.Equals("email", StringComparison.CurrentCultureIgnoreCase));
        if (emailPreference == null)
        {
            return new Unit();
        }
        
        var preferenceId = emailPreference.PreferenceId;
        var vacancyTask = recruitApiClient.Get<GetLiveVacancyApiResponse>(new GetLiveVacancyApiRequest(request.VacancyReference));
        var candidatesTask = candidateApiClient.Get<GetCandidateApplicationApiResponse>(new GetCandidateApplicationsByVacancyRequest(request.VacancyReference.ToString(), preferenceId));
        var allCandidateApplicationsTask = candidateApiClient.Get<GetCandidateApplicationApiResponse>(new GetCandidateApplicationsByVacancyRequest(request.VacancyReference.ToString(), preferenceId, false));

        await Task.WhenAll(vacancyTask, candidatesTask, allCandidateApplicationsTask);

        var notificationTasks = new List<Task>();
        foreach (var candidate in candidatesTask.Result.Candidates)
        {
            var email = new SendVacancyClosedEarlyTemplate(
                helper.VacancyClosedEarlyTemplateId,
                candidate.Candidate.Email,
                candidate.Candidate.FirstName,
                vacancyTask.Result.Title,
                helper.VacancyUrl,
                vacancyTask.Result.EmployerName,
                vacancyTask.Result.EmployerLocation.AddressLine4 ?? vacancyTask.Result.EmployerLocation.AddressLine3 ?? vacancyTask.Result.EmployerLocation.AddressLine2 ?? vacancyTask.Result.EmployerLocation.AddressLine1, 
                vacancyTask.Result.EmployerLocation?.Postcode, 
                candidate.ApplicationCreatedDate,
                helper.SettingsUrl);
            notificationTasks.Add(notificationService.Send(new SendEmailCommand(email.TemplateId, email.RecipientAddress, email.Tokens)));
        }

        await Task.WhenAll(notificationTasks);

        var updateCandidate = new List<Task>();
        foreach (var candidate in allCandidateApplicationsTask.Result.Candidates)
        {
            var jsonPatchDocument = new JsonPatchDocument<Domain.Models.Application>();
            jsonPatchDocument.Replace(x => x.Status, ApplicationStatus.Expired);
            var patchRequest = new PatchApplicationApiRequest(candidate.ApplicationId, candidate.Candidate.Id, jsonPatchDocument);
            updateCandidate.Add(candidateApiClient.PatchWithResponseCode(patchRequest));
        }

        await Task.WhenAll(updateCandidate);
        
        
        return new Unit();
    }
}
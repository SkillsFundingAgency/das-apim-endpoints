using MediatR;
using SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates;
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

        await Task.WhenAll(vacancyTask, candidatesTask);

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
            await notificationService.Send(new SendEmailCommand(email.TemplateId, email.RecipientAddress, email.Tokens));
        }
        
        return new Unit();
    }
}
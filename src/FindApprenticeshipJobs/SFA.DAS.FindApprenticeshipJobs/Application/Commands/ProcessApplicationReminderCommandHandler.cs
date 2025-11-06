using MediatR;
using SFA.DAS.FindApprenticeshipJobs.Application.Shared;
using SFA.DAS.FindApprenticeshipJobs.Domain.Constants;
using SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Commands;

public class ProcessApplicationReminderCommandHandler(
    IRecruitApiClient<RecruitApiV2Configuration> recruitApiClient, 
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient,
    EmailEnvironmentHelper helper,
    INotificationService notificationService) : IRequestHandler<ProcessApplicationReminderCommand, Unit>
{
    public async Task<Unit> Handle(ProcessApplicationReminderCommand request, CancellationToken cancellationToken)
    {
        var preference = await candidateApiClient.Get<GetPreferencesApiResponse>(new GetCandidatePreferencesRequest());
        var emailPreference = preference.Preferences
            .FirstOrDefault(c => c.PreferenceType.Equals("closing", StringComparison.CurrentCultureIgnoreCase));
        if (emailPreference == null)
        {
            return new Unit();
        }
        
        var preferenceId = emailPreference.PreferenceId;
        var vacancyTask = recruitApiClient.Get<GetLiveVacancyApiResponse>(new GetLiveVacancyApiRequest(request.VacancyReference));
        var candidatesTask = candidateApiClient.Get<GetCandidateApplicationApiResponse>(new GetCandidateApplicationsByVacancyRequest(request.VacancyReference.ToString(), preferenceId));

        await Task.WhenAll(vacancyTask, candidatesTask);

        var vacancy = vacancyTask.Result;
        var candidates = candidatesTask.Result;

        var employmentWorkLocation = vacancy.EmployerLocationOption switch
        {
            AvailableWhere.AcrossEngland => EmailTemplateBuilderConstants.RecruitingNationally,
            AvailableWhere.MultipleLocations => EmailTemplateAddressExtension.GetEmploymentLocationCityNames(vacancy.OtherAddresses),
            AvailableWhere.OneLocation => EmailTemplateAddressExtension.GetOneLocationCityName(vacancy.Address),
            _ => EmailTemplateAddressExtension.GetOneLocationCityName(vacancy.Address)
        };

        foreach (var email in candidates.Candidates.Select(candidate => new SendSubmitApplicationEmailReminderTemplate(
                     helper.ApplicationReminderEmailTemplateId,
                     candidate.Candidate.Email,
                     candidate.Candidate.FirstName,
                     request.DaysUntilClosing,
                     vacancy.Title,
                     helper.VacancyUrl,
                     vacancy.EmployerName,
                     employmentWorkLocation,
                     helper.CandidateApplicationUrl,
                     vacancy.ClosingDate,
                     helper.SettingsUrl)))
        {
            await notificationService.Send(new SendEmailCommand(email.TemplateId, email.RecipientAddress, email.Tokens));
        }

        return new Unit();
    }
}
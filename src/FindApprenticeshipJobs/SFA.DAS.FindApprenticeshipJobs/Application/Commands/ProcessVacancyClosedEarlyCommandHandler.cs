using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.FindApprenticeshipJobs.Application.Shared;
using SFA.DAS.FindApprenticeshipJobs.Domain.Constants;
using SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
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
        var vacancyTask = recruitApiClient.Get<GetLiveVacancyApiResponse>(new GetLiveVacancyApiRequest(request.VacancyReference));
        
        var allCandidateApplicationsTask = candidateApiClient.Get<GetCandidateApplicationApiResponse>(new GetCandidateApplicationsByVacancyRequest(request.VacancyReference.ToString(), null, false));

        await Task.WhenAll(vacancyTask, allCandidateApplicationsTask);
        var notificationTasks = new List<Task>();
        var updateCandidate = new List<Task>();

        var employmentWorkLocation = vacancyTask.Result.EmployerLocationOption switch
        {
            AvailableWhere.AcrossEngland => EmailTemplateBuilderConstants.RecruitingNationally,
            AvailableWhere.MultipleLocations => EmailTemplateAddressExtension.GetEmploymentLocationCityNames(vacancyTask.Result.OtherAddresses),
            AvailableWhere.OneLocation => EmailTemplateAddressExtension.GetOneLocationCityName(vacancyTask.Result.Address),
            _ => EmailTemplateAddressExtension.GetOneLocationCityName(vacancyTask.Result.Address)
        };

        foreach (var candidate in allCandidateApplicationsTask.Result.Candidates)
        {
            var jsonPatchDocument = new JsonPatchDocument<Domain.Models.Application>();
            jsonPatchDocument.Replace(x => x.Status, ApplicationStatus.Expired);
            var patchRequest = new PatchApplicationApiRequest(candidate.ApplicationId, candidate.Candidate.Id, jsonPatchDocument);
            updateCandidate.Add(candidateApiClient.PatchWithResponseCode(patchRequest));
            var email = new SendVacancyClosedEarlyTemplate(
                helper.VacancyClosedEarlyTemplateId,
                candidate.Candidate.Email,
                candidate.Candidate.FirstName,
                vacancyTask.Result.Title,
                helper.VacancyUrl,
                vacancyTask.Result.EmployerName,
                employmentWorkLocation, 
                candidate.ApplicationCreatedDate,
                helper.SettingsUrl);
            notificationTasks.Add(notificationService.Send(new SendEmailCommand(email.TemplateId, email.RecipientAddress, email.Tokens)));
        }

        await Task.WhenAll(notificationTasks);
        await Task.WhenAll(updateCandidate);
        
        
        return new Unit();
    }
}
using System.Net;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using Polly;
using Polly.Retry;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Commands;

public class ProcessVacancyClosedEarlyCommandHandler(
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient, 
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient,
    EmailEnvironmentHelper helper,
    INotificationService notificationService,
    ILogger<ProcessVacancyClosedEarlyCommandHandler> logger) : IRequestHandler<ProcessVacancyClosedEarlyCommand, Unit>
{
    public async Task<Unit> Handle(ProcessVacancyClosedEarlyCommand request, CancellationToken cancellationToken)
    {
        var closedVacancyNotFoundPolicy = GetClosedVacancyNotFoundPolicy(request);

        var vacancy = await closedVacancyNotFoundPolicy.ExecuteAsync(
            () => recruitApiClient.GetWithResponseCode<GetClosedVacancyApiResponse>(
                new GetClosedVacancyApiRequest(request.VacancyReference)));

        if (vacancy.StatusCode == HttpStatusCode.NotFound)
        {
            throw new Exception($"Vacancy not found: {request.VacancyReference} while processing closed vacancy handler");
        }
        
        var allCandidateApplications = await candidateApiClient.Get<GetCandidateApplicationApiResponse>(new GetCandidateApplicationsByVacancyRequest(request.VacancyReference.ToString(), null, false));

        await Task.WhenAll(vacancyTask, allCandidateApplicationsTask);
        var notificationTasks = new List<Task>();
        var updateCandidate = new List<Task>();

        var vacancy = vacancyTask.Result;
        var allCandidateApplications = allCandidateApplicationsTask.Result;

        var employmentWorkLocation = vacancy.EmployerLocationOption switch
        {
            AvailableWhere.AcrossEngland => EmailTemplateBuilderConstants.RecruitingNationally,
            AvailableWhere.MultipleLocations => EmailTemplateAddressExtension.GetEmploymentLocationCityNames(vacancy.OtherAddresses),
            AvailableWhere.OneLocation => EmailTemplateAddressExtension.GetOneLocationCityName(vacancy.Address),
            _ => EmailTemplateAddressExtension.GetOneLocationCityName(vacancy.Address)
        };

        foreach (var candidate in allCandidateApplications.Candidates)
        {
            var jsonPatchDocument = new JsonPatchDocument<Domain.Models.Application>();
            jsonPatchDocument.Replace(x => x.Status, ApplicationStatus.Expired);
            var patchRequest = new PatchApplicationApiRequest(candidate.ApplicationId, candidate.Candidate.Id, jsonPatchDocument);
            updateCandidate.Add(candidateApiClient.PatchWithResponseCode(patchRequest));
            var email = new SendVacancyClosedEarlyTemplate(
                helper.VacancyClosedEarlyTemplateId,
                candidate.Candidate.Email,
                candidate.Candidate.FirstName,
                vacancy.Body.Title,
                helper.VacancyUrl,
                vacancy.EmployerName,
                employmentWorkLocation, 
                candidate.ApplicationCreatedDate,
                helper.SettingsUrl);
            notificationTasks.Add(notificationService.Send(new SendEmailCommand(email.TemplateId, email.RecipientAddress, email.Tokens)));
        }

        await Task.WhenAll(notificationTasks);
        await Task.WhenAll(updateCandidate);
        
        return new Unit();
    }

    private AsyncRetryPolicy<ApiResponse<GetClosedVacancyApiResponse>> GetClosedVacancyNotFoundPolicy(ProcessVacancyClosedEarlyCommand request)
    {
        return Policy
            .HandleResult<ApiResponse<GetClosedVacancyApiResponse>>(r => r.StatusCode == HttpStatusCode.NotFound)
            .WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(4), 
                (_, _, retryCount, _) =>
                {
                    logger.LogInformation($"ProcessVacancyClosedEarlyCommandHandler: Unable to find {request.VacancyReference}. Retry {retryCount} due to 404 response");
                });
    }
}
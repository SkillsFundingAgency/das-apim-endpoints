using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Commands;

public class ProcessVacancyClosedEarlyCommandHandler(
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient, 
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient,
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
        
        var allCandidateApplications = await candidateApiClient.Get<GetCandidateApplicationApiResponse>(
            new GetCandidateApplicationsByVacancyRequest(request.VacancyReference.ToString(),
                null,
                false));

        var updateCandidate = new List<Task>();

        foreach (var candidate in allCandidateApplications.Candidates)
        {
            var jsonPatchDocument = new JsonPatchDocument<Domain.Models.Application>();
            jsonPatchDocument.Replace(x => x.Status, ApplicationStatus.Expired);
            var patchRequest = new PatchApplicationApiRequest(candidate.ApplicationId, candidate.Candidate.Id, jsonPatchDocument);
            updateCandidate.Add(candidateApiClient.PatchWithResponseCode(patchRequest));
        }

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
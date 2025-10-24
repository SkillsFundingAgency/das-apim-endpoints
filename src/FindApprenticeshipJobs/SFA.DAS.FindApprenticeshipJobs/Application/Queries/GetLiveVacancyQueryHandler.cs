using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries;

public class GetLiveVacancyQueryHandler(
    IRecruitApiClient<RecruitApiV2Configuration> recruitApiClient,
    ILiveVacancyMapper liveVacancyMapper,
    ICourseService courseService,
    ILogger<GetLiveVacancyQueryHandler> logger)
    : IRequestHandler<GetLiveVacancyQuery, GetLiveVacancyQueryResult>
{
    public async Task<GetLiveVacancyQueryResult> Handle(GetLiveVacancyQuery request, CancellationToken cancellationToken)
    {
        var liveVacancyNotFoundPolicy = GetLiveVacancyNotFoundPolicy(request);

        var vacancy = await liveVacancyNotFoundPolicy.ExecuteAsync(
            () => recruitApiClient.GetWithResponseCode<GetLiveVacancyApiResponse>(
                new GetLiveVacancyApiRequest(request.VacancyReference)));

        if (vacancy.StatusCode == HttpStatusCode.NotFound)
        {
            throw new Exception($"Vacancy not found: {request.VacancyReference} while processing live vacancy handler");
        }
        
        var standards = await courseService.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse));
        
        var result = liveVacancyMapper.Map(vacancy.Body, standards);

        return new GetLiveVacancyQueryResult
        {
            LiveVacancy = result
        };

    }
    private AsyncRetryPolicy<ApiResponse<GetLiveVacancyApiResponse>> GetLiveVacancyNotFoundPolicy(GetLiveVacancyQuery request)
    {
        return Policy
            .HandleResult<ApiResponse<GetLiveVacancyApiResponse>>(r => r.StatusCode == HttpStatusCode.NotFound)
            .WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(4), 
                (_, _, retryCount, _) =>
                {
                    logger.LogInformation("GetLiveVacancyQueryHandler: Unable to find {RequestVacancyReference}. Retry {RetryCount} due to 404 response", request.VacancyReference, retryCount);
                });
    }
}
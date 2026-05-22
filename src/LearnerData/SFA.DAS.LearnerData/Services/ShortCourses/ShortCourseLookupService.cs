using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Constants;
using SFA.DAS.SharedOuterApi.Types.Extensions;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.LearnerData.Services.ShortCourses;

public interface IShortCourseLookupService
{
    Task<ShortCourseLookupResult> GetCourseDetails(string courseCode, DateTime startDate);
}

public class ShortCourseLookupService : IShortCourseLookupService
{
    private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
    private readonly ILogger<ShortCourseLookupService> _logger;
    private readonly AsyncRetryPolicy<ApiResponse<CourseLookupDetailResponse>> _retryPolicy;

    public ShortCourseLookupService(
        ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
        ILogger<ShortCourseLookupService> logger)
    {
        _coursesApiClient = coursesApiClient;
        _logger = logger;
        _retryPolicy = Policy
            .Handle<HttpRequestException>()
            .Or<TaskCanceledException>()
            .OrResult<ApiResponse<CourseLookupDetailResponse>>(r => (int)r.StatusCode >= 500)
            .WaitAndRetryAsync(3,
                attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: (_, delay, attempt, _) =>
                    logger.LogWarning("Courses API transient error for retry {Attempt}. Waiting {Delay}s before next attempt.", attempt, delay.TotalSeconds));
    }

    public async Task<ShortCourseLookupResult> GetCourseDetails(string courseCode, DateTime startDate)
    {
        var apiResponse = await _retryPolicy.ExecuteAsync(
            () => _coursesApiClient.GetWithResponseCode<CourseLookupDetailResponse>(
                new GetCourseLookupDetailsByIdRequest(courseCode)));

        if (!apiResponse.StatusCode.IsSuccessStatusCode())
            throw new InvalidOperationException($"Courses API returned {apiResponse.StatusCode} for course {courseCode}.");

        var response = apiResponse.Body;

        if (response == null)
            throw new InvalidOperationException($"Courses API returned no data for course {courseCode}.");

        var price = response.ApprenticeshipFunding.MaxFundingOn(startDate);

        if (price == 0)
            throw new InvalidOperationException($"No funding band found for course {courseCode} on start date {startDate:yyyy-MM-dd}.");

        if (!Enum.TryParse<LearningType>(response.LearningType, out var learningType))
            throw new InvalidOperationException($"Unrecognised learning type '{response.LearningType}' for course {courseCode}.");

        return new ShortCourseLookupResult { Price = price, LearningType = learningType };
    }
}

public class ShortCourseLookupResult
{
    public int Price { get; set; }
    public LearningType LearningType { get; set; }
}

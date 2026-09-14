using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using SFA.DAS.SharedOuterApi.Types.Constants;
using SFA.DAS.SharedOuterApi.Types.Extensions;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.LearnerData.Services.ShortCourses;

public interface IShortCourseLookupService
{
    Task<ShortCourseLookupResult> GetCourseDetails(string courseCode, DateTime startDate);
}

public class ShortCourseLookupService : IShortCourseLookupService
{
    private readonly ICourseService _courseService;
    private readonly ILogger<ShortCourseLookupService> _logger;
    private readonly AsyncRetryPolicy<CourseLookupResult> _retryPolicy;

    public ShortCourseLookupService(
        ICourseService courseService,
        ILogger<ShortCourseLookupService> logger)
    {
        _courseService = courseService;
        _logger = logger;
        _retryPolicy = Policy
            .Handle<HttpRequestException>()
            .Or<TaskCanceledException>()
            .OrResult<CourseLookupResult>(r => r.Status == CourseLookupStatus.Unavailable)
            .WaitAndRetryAsync(3,
                attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: (_, delay, attempt, _) =>
                    logger.LogWarning("Courses API transient error for retry {Attempt}. Waiting {Delay}s before next attempt.", attempt, delay.TotalSeconds));
    }

    public async Task<ShortCourseLookupResult> GetCourseDetails(string courseCode, DateTime startDate)
    {
        CourseLookupResult lookupResult;
        try
        {
            lookupResult = await _retryPolicy.ExecuteAsync(
                () => _courseService.GetCourseLookupDetailsById(courseCode));
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            throw new CoursesApiUnavailableException($"Courses API unavailable for course {courseCode} after retries.", ex);
        }

        switch (lookupResult.Status)
        {
            case CourseLookupStatus.Unavailable:
                throw new CoursesApiUnavailableException($"Courses API unavailable for course {courseCode} after retries.");
            case CourseLookupStatus.NotFound:
                throw new InvalidCourseException($"Courses API could not find course {courseCode}.");
        }

        var response = lookupResult.Course;

        if (response == null)
            throw new InvalidOperationException($"Courses API returned no data for course {courseCode}.");

        var price = response.ApprenticeshipFunding.MaxFundingOn(startDate);

        if (price == 0)
            throw new InvalidCourseException($"No funding band found for course {courseCode} on start date {startDate:yyyy-MM-dd}.");

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

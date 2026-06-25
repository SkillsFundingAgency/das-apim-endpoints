using Microsoft.Extensions.Logging;
using Polly.Retry;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Models;
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
    private readonly ICourseService _courseService;
    private readonly ILogger<ShortCourseLookupService> _logger;
    private readonly AsyncRetryPolicy<ApiResponse<CourseLookupDetailResponse>> _retryPolicy;

    public ShortCourseLookupService(
        ICourseService courseService,
        ILogger<ShortCourseLookupService> logger)
    {
        _courseService = courseService;
        _logger = logger;
    }

    public async Task<ShortCourseLookupResult> GetCourseDetails(string courseCode, DateTime startDate)
    {
        var response = await _courseService.GetCourseLookupDetailsById(courseCode);

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

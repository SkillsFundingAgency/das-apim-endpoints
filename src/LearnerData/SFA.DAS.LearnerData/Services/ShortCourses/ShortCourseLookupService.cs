using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Common;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.Services.ShortCourses;

public interface IShortCourseLookupService
{
    Task<ShortCourseLookupResult> GetCourseDetails(string courseCode, DateTime startDate);
}

public class ShortCourseLookupService(
    ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
    ILogger<ShortCourseLookupService> logger) : IShortCourseLookupService
{
    public async Task<ShortCourseLookupResult> GetCourseDetails(string courseCode, DateTime startDate)
    {
        try
        {
            var response = await coursesApiClient.Get<CourseLookupDetailResponse>(new GetCourseLookupDetailsByIdRequest(courseCode));

            if (response == null)
            {
                logger.LogError("Courses API returned no data for course {CourseCode}. Using default values", courseCode);
                return Defaults();
            }

            var price = response.ApprenticeshipFunding.MaxFundingOn(startDate);

            if (price == 0)
            {
                logger.LogWarning("No funding band found for course {CourseCode} on start date {StartDate}. Using default values", courseCode, startDate);
                return Defaults();
            }

            if (!Enum.TryParse<LearningType>(response.LearningType, out var learningType))
            {
                logger.LogWarning("Unrecognised learning type '{LearningType}' for course {CourseCode}. Defaulting to ApprenticeshipUnit", response.LearningType, courseCode);
                learningType = LearningType.ApprenticeshipUnit;
            }

            return new ShortCourseLookupResult { Price = price, LearningType = learningType };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve course details for course {CourseCode}. Using default values", courseCode);
            return Defaults();
        }
    }

    private static ShortCourseLookupResult Defaults() => new() { Price = 0, LearningType = LearningType.ApprenticeshipUnit };
}

public class ShortCourseLookupResult
{
    public int Price { get; set; }
    public LearningType LearningType { get; set; }
}

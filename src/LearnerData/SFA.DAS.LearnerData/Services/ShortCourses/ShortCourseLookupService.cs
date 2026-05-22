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

public class ShortCourseLookupService(
    ICoursesApiClient<CoursesApiConfiguration> coursesApiClient) : IShortCourseLookupService
{
    public async Task<ShortCourseLookupResult> GetCourseDetails(string courseCode, DateTime startDate)
    {
        var response = await coursesApiClient.Get<CourseLookupDetailResponse>(new GetCourseLookupDetailsByIdRequest(courseCode));

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

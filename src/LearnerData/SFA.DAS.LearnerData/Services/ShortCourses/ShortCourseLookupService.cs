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

public class ShortCourseLookupService(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient) : IShortCourseLookupService
{
    public async Task<ShortCourseLookupResult> GetCourseDetails(string courseCode, DateTime startDate)
    {
        var response = await coursesApiClient.Get<CourseLookupDetailResponse>(new GetCourseLookupDetailsByIdRequest(courseCode));

        Enum.TryParse<LearningType>(response.LearningType, out var learningType);

        return new ShortCourseLookupResult
        {
            Price = response.ApprenticeshipFunding.MaxFundingOn(startDate),
            LearningType = learningType
        };
    }
}

public class ShortCourseLookupResult
{
    public int Price { get; set; }
    public LearningType LearningType { get; set; }
}

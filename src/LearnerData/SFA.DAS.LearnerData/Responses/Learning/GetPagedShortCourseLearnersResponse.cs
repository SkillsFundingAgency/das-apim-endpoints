using SFA.DAS.LearnerData.Responses;
using ShortCourseLearning = SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning.GetShortCourseLearnersForEarningsResponse.Learning;

namespace SFA.DAS.LearnerData.Responses.Learning;

public class GetPagedShortCourseLearnersResponse : PagedQueryResult<ShortCourseLearning>
{
}

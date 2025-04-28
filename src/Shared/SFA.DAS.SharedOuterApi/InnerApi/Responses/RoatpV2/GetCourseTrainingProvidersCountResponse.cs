using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;

public sealed class GetCourseTrainingProvidersCountResponse
{
    public List<CourseTrainingProviderCountModel> Courses { get; set; } = new List<CourseTrainingProviderCountModel>();
}

public sealed class CourseTrainingProviderCountModel
{
    public int LarsCode { get; set; }
    public int ProvidersCount { get; set; }
    public int TotalProvidersCount { get; set; }
}
